﻿using LazyTransportProtocol.Core.Application.Infrastructure;
using LazyTransportProtocol.Core.Application.Protocol;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.Model;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Common;
using LazyTransportProtocol.Core.Domain.Exceptions;
using LazyTransportProtocol.Core.Domain.Exceptions.Authorization;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LazyTransportProtocol.Server
{
	public delegate TResponse RequestCallback<TResponse>(IProtocolRequest<TResponse> request)
		where TResponse : class, IProtocolResponse, new();

	public class ProtocolRequestListener
	{
		private static ManualResetEvent allDone = new ManualResetEvent(false);

		private static ProtocolRequestExecutor executor = new ProtocolRequestExecutor();
		private const string eof = "<EOF>";

		public void Listen(IPAddress ipAddress, int port)
		{
			IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

			Socket listener = new Socket(ipAddress.AddressFamily,
				SocketType.Stream, ProtocolType.Tcp);

			listener.Bind(localEndPoint);
			listener.Listen(100);

			while (true)
			{
				allDone.Reset();

				Console.WriteLine("Waiting for a connection...");
				listener.BeginAccept(OnClientConnected, listener);

				allDone.WaitOne();
			}
		}

		public static void OnClientConnected(IAsyncResult ar)
		{
			allDone.Set();

			Socket listener = (Socket)ar.AsyncState;
			Socket handler = listener.EndAccept(ar);

			// Create the state object.
			StateObject state = new StateObject
			{
				WorkSocket = handler
			};

			handler.BeginReceive(state.Buffer, 0, state.Buffer.Length, 0, OnDataReceived, state);
		}

		public static void OnDataReceived(IAsyncResult ar)
		{
			try
			{
				IDecoder decoder = new ProtocolDecoder();

				StateObject state = (StateObject)ar.AsyncState;
				Socket handler = state.WorkSocket;

				int bytesRead = handler.EndReceive(ar);

				if (bytesRead == 0)
				{
					return;
				}

				string decoded = decoder.Decode(state.Buffer, 0, bytesRead);
				state.StringBuilder.Append(decoded);

				if (decoded.EndsWith(eof))
				{
					state.StringBuilder.Remove(state.StringBuilder.Length - eof.Length, eof.Length);
					string requestString = state.StringBuilder.ToString();

					string serializedResponse;

					if (state.ProtocolState == null)
					{
						// First client's request. Hanshake expected
						serializedResponse = BeginHandshake(requestString, state);
					}
					else
					{
						serializedResponse = HandleRequest(requestString, (ProtocolState)state.ProtocolState);
					}

					Send(state, serializedResponse + eof);
				}
				else
				{
					// Not all data received. Get more.
					handler.BeginReceive(state.Buffer, 0, state.Buffer.Length, 0, OnDataReceived, state);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		private static string BeginHandshake(string requestString, StateObject state)
		{
			ProtocolVersion handshakeProtocol = ProtocolVersion.Handshake;

			// Default headers for handshake
			AgreedHeadersDictionary handshakeHeaders = new AgreedHeadersDictionary(";", 1024, handshakeProtocol);
			MessageHeadersDictionary responseHeaders = new MessageHeadersDictionary();
			MediumDeserializedObject requestObject = null;
			try
			{
				requestObject = ProtocolDeserializer.Deserialize(requestString, handshakeHeaders, handshakeProtocol);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);

				return String.Empty;
			}

			HandshakeRequest request = ProtocolBodyDeserializer.Deserialize<HandshakeRequest>(requestObject.Body, ProtocolVersion.Handshake);
			var response = executor.Execute(request);

			if (response.IsSuccessful)
			{
				ProtocolVersion protocolVersion = new ProtocolVersion(request.ProtocolVersion);

				state.ProtocolState = new ProtocolState
				{
					AgreedHeaders = new AgreedHeadersDictionary(request.Separator, request.BufferSize, protocolVersion)
				};

				state.Buffer = new byte[request.BufferSize];
			}

			string identifier = response.GetIdentifier(handshakeProtocol);
			string headers = ProtocolMessageHeaderSerializer.Serialize(responseHeaders, handshakeProtocol);
			string responseBody = ProtocolBodySerializer.Serialize(response, handshakeProtocol);
			string serializedResponse = ProtocolSerializer.Serialize(identifier, headers, responseBody, handshakeHeaders);

			return serializedResponse;
		}

		private static string HandleRequest(string requestString, ProtocolState connectionState)
		{
			ProtocolVersion protocolVersion = connectionState.AgreedHeaders.ProtocolVersion;
			MediumDeserializedObject requestObject = null;
			try
			{
				requestObject = ProtocolDeserializer.Deserialize(requestString, connectionState.AgreedHeaders, protocolVersion);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);

				return String.Empty;
			}

			IProtocolResponse protocolResponse = null;
			IProtocolRequest<IProtocolResponse> request = null;

			if (connectionState.AuthenticationContext == null)
			{
				if (requestObject.ControlCommand != AuthenticationRequest.Identifier)
				{
					protocolResponse = new ErrorResponse
					{
						Code = 403,
						Message = "Unauthorized."
					};
				}
				else
				{
					Deserialize<AuthenticationRequest>();

					if (request.AuthenticationContext != null)
					{
						connectionState.AuthenticationContext = request.AuthenticationContext;
					}
				}
			}
			else
			{
				switch (requestObject.ControlCommand)
				{
					case CreateUserRequest.Identifier:
						Deserialize<CreateUserRequest>();
						break;

					case DeleteUserRequest.Identifier:
						Deserialize<DeleteUserRequest>();
						break;

					case AuthenticationRequest.Identifier:
						Deserialize<AuthenticationRequest>();
						break;

					case ListDirectoryClientRequest.Identifier:
						Deserialize<ListDirectoryClientRequest>();
						break;

					case DownloadFileRequest.Identifier:
						Deserialize<DownloadFileRequest>();
						break;

					case CreateDirectoryRequest.Identifier:
						Deserialize<CreateDirectoryRequest>();
						break;

					case UploadFileRequest.Identifier:
						Deserialize<UploadFileRequest>();
						break;

					default:
						protocolResponse = new ErrorResponse
						{
							Code = 400,
							Message = "Unsupported request."
						};
						break;
				}
			}

			void Deserialize<TRequest>()
				where TRequest : IProtocolRequest<IProtocolResponse>
			{
				try
				{
					request = ProtocolBodyDeserializer.Deserialize<TRequest>(requestObject.Body, protocolVersion);
					request.AuthenticationContext = connectionState.AuthenticationContext;
					protocolResponse = executor.Execute(request);
				}
				catch (AuthorizationException)
				{
					protocolResponse = new ErrorResponse
					{
						Code = 403,
						Message = "Unauthorized."
					};
				}
				catch (CustomException e)
				{
					protocolResponse = new ErrorResponse
					{
						Code = 400,
						Message = e.Message
					};
				}
				catch (Exception)
				{
					protocolResponse = new ErrorResponse
					{
						Code = 500,
						Message = "Internal server error."
					};
				}
			}

			MessageHeadersDictionary responseHeaders = new MessageHeadersDictionary();

			string identifier = protocolResponse.GetIdentifier(protocolVersion);
			string headers = ProtocolMessageHeaderSerializer.Serialize(responseHeaders, protocolVersion);
			string serializedBody = ProtocolBodySerializer.Serialize(protocolResponse, protocolVersion);
			string serializedResponse = ProtocolSerializer.Serialize(identifier, headers, serializedBody, connectionState.AgreedHeaders);

			return serializedResponse;
		}

		private static void Send(StateObject state, string data)
		{
			IEncoder encoder = new ProtocolEncoder();

			byte[] byteData = encoder.Encode(data);

			state.WorkSocket.BeginSend(byteData, 0, byteData.Length, 0, OnSendCompleted, state);
		}

		private static void OnSendCompleted(IAsyncResult ar)
		{
			try
			{
				StateObject state = (StateObject)ar.AsyncState;
				Socket handler = state.WorkSocket;

				int bytesSent = handler.EndSend(ar);

				if (state.ProtocolState != null)
				{
					state.StringBuilder.Clear();
					handler.BeginReceive(state.Buffer, 0, state.Buffer.Length, 0, OnDataReceived, state);
				}
				else
				{
					// Unsuccessful handshake
					handler.Shutdown(SocketShutdown.Both);
					handler.Close();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}

		public class StateObject
		{
			public Socket WorkSocket { get; set; }

			public byte[] Buffer { get; set; } = new byte[1024];

			public StringBuilder StringBuilder { get; } = new StringBuilder();

			public IConnectionState ProtocolState { get; set; }
		}
	}
}
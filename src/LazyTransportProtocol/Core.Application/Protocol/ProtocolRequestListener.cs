using LazyTransportProtocol.Core.Application.Infrastructure;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.Metadata;
using LazyTransportProtocol.Core.Application.Protocol.Model;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Common;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using LazyTransportProtocol.Core.Domain.Exceptions.Authorization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LazyTransportProtocol.Core.Application.Protocol
{
	public delegate TResponse RequestCallback<TResponse>(IProtocolRequest<TResponse> request)
		where TResponse : class, IProtocolResponse, new();

	public class ProtocolRequestListener
	{
		private static ManualResetEvent allDone = new ManualResetEvent(false);

		private readonly IEncoder _encoder = new ProtocolEncoder();
		private readonly IDecoder _decoder = new ProtocolDecoder();

		public void Listen(int port)
		{
			IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
			IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

			Socket listener = new Socket(ipAddress.AddressFamily,
				SocketType.Stream, ProtocolType.Tcp);

			listener.Bind(localEndPoint);
			listener.Listen(100);

			while (true)
			{
				allDone.Reset();

				Console.WriteLine("Waiting for a connection...");
				listener.BeginAccept(
					new AsyncCallback(OnClientConnected),
					listener);

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

			handler.BeginReceive(state.Buffer, 0, state.Buffer.Length, 0,
			new AsyncCallback(OnDataReceived), state);
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

				string requestString = state.StringBuilder.ToString();

				if (requestString.EndsWith("<EOF>"))
				{
					// All the data has been read
					Console.WriteLine("Read {0} bytes from socket.",
						requestString.Length);

					requestString = requestString.Replace("<EOF>", "");

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

					Send(state, serializedResponse);
				}
				else
				{
					// Not all data received. Get more.
					handler.BeginReceive(state.Buffer, 0, state.Buffer.Length, 0,
						new AsyncCallback(OnDataReceived), state);
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
			MediumDeserializedObject requestObject = new ProtocolDeserializer().Deserialize(requestString, handshakeHeaders, handshakeProtocol);

			HandshakeRequest request = ProtocolBodyDeserializer.Deserialize<HandshakeRequest>(requestObject.Body, ProtocolVersion.Handshake);
			var response = new ProtocolRequestExecutor().Execute(request);

			if (response.IsSuccessful)
			{
				state.ProtocolState = new ProtocolState
				{
					AgreedHeaders = new AgreedHeadersDictionary(request.Separator, request.BufferSize, request.ProtocolVersion)
				};

				state.Buffer = new byte[request.BufferSize];
			}

			string identifier = response.GetIdentifier(handshakeProtocol);
			string headers = new ProtocolMessageHeaderSerializer().Serialize(responseHeaders, handshakeProtocol);
			string responseBody = new ProtocolBodySerializer().Serialize(response, handshakeProtocol);
			string serializedResponse = new ProtocolSerializer().Serialize(identifier, headers, responseBody, handshakeHeaders);

			return serializedResponse;
		}

		private static string HandleRequest(string requestString, ProtocolState connectionState)
		{
			ProtocolVersion protocolVersion = connectionState.AgreedHeaders.ProtocolVersion;

			MediumDeserializedObject requestObject = new ProtocolDeserializer().Deserialize(requestString, connectionState.AgreedHeaders, protocolVersion);
			ProtocolRequestExecutor executor = new ProtocolRequestExecutor();

			IProtocolResponse protocolResponse = null;
			IProtocolRequest<IProtocolResponse> request = null;

			if (connectionState.AuthenticationContext == null && requestObject.ControlCommand != AuthenticationRequest.Identifier)
			{
				throw new AuthorizationException();
			}

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
			}

			void Deserialize<TRequest>()
				where TRequest : IProtocolRequest<IProtocolResponse>
			{
				request = ProtocolBodyDeserializer.Deserialize<TRequest>(requestObject.Body, protocolVersion);
				request.AuthenticationContext = connectionState.AuthenticationContext;
				protocolResponse = executor.Execute(request);
			}

			MessageHeadersDictionary responseHeaders = new MessageHeadersDictionary();

			string identifier = protocolResponse.GetIdentifier(protocolVersion);
			string headers = new ProtocolMessageHeaderSerializer().Serialize(responseHeaders, protocolVersion);
			string serializedBody = new ProtocolBodySerializer().Serialize(protocolResponse, protocolVersion);
			string serializedResponse = new ProtocolSerializer().Serialize(identifier, headers, serializedBody, connectionState.AgreedHeaders);

			return serializedResponse;
		}

		private static void Send(StateObject state, string data)
		{
			IEncoder encoder = new ProtocolEncoder();

			byte[] byteData = encoder.Encode(data);

			state.WorkSocket.BeginSend(byteData, 0, byteData.Length, 0,
				new AsyncCallback(OnSendCompleted), state);
		}

		private static void OnSendCompleted(IAsyncResult ar)
		{
			try
			{
				StateObject state = (StateObject)ar.AsyncState;
				Socket handler = state.WorkSocket;

				int bytesSent = handler.EndSend(ar);
				Console.WriteLine("Sent {0} bytes to client.", bytesSent);

				if (state.ProtocolState != null)
				{
					state.StringBuilder.Clear();
					handler.BeginReceive(state.Buffer, 0, state.Buffer.Length, 0,
						new AsyncCallback(OnDataReceived), state);
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
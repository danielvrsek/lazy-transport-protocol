using LazyTransportProtocol.Core.Application.Infrastructure;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.Metadata;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Common;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
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

			handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
				new AsyncCallback(OnDataReceived), state);
		}

		public static void OnDataReceived(IAsyncResult ar)
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

			if (requestString.IndexOf("<EOF>") > -1)
			{
				// All the data has been read
				Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
					requestString.Length, requestString);

				requestString = requestString.Replace("<EOF>", "");

				if (state.ProtocolState == null)
				{
					ProtocolVersion handshakeProtocol = ProtocolVersion.Handshake;

					// Temporary headers for handshake
					AgreedHeadersDictionary handshakeHeaders = new AgreedHeadersDictionary(";", handshakeProtocol);
					MediumDeserializedObject requestObject = DeserializeHelper.DeserializeRequestString(requestString, handshakeHeaders, handshakeProtocol);

					HandshakeRequest request = ProtocolBodyDeserializer.Deserialize<HandshakeRequest>(requestObject.Body, ProtocolVersion.Handshake);
					var response = new ProtocolRequestExecutor().Execute(request);

					if (response.IsSuccessful)
					{
						state.ProtocolState = new ProtocolState
						{
							AgreedHeaders = new AgreedHeadersDictionary(request.Separator, request.ProtocolVersion)
						};
					}

					string responseBody = ProtocolBodySerializer.Serialize(response, handshakeProtocol);
					string serializedResponse = SerializeHelper.SerializeRequestString(response.GetIdentifier(handshakeProtocol), responseBody, handshakeHeaders, handshakeProtocol);

					Send(state, serializedResponse);
				}
				else
				{
					ProtocolState connectionState = (ProtocolState)state.ProtocolState;
					ProtocolVersion protocolVersion = connectionState.AgreedHeaders.ProtocolVersion;

					MediumDeserializedObject requestObject = DeserializeHelper.DeserializeRequestString(requestString, connectionState.AgreedHeaders, protocolVersion);
					ProtocolRequestExecutor executor = new ProtocolRequestExecutor();

					IProtocolResponse protocolResponse = null;
					IRequest<IResponse> request = null;

					switch (requestObject.ControlCommand)
					{
						case CreateUserRequest.Identifier:
							request = ProtocolBodyDeserializer.Deserialize<CreateUserRequest>(requestObject.Body, protocolVersion);
							protocolResponse = (IProtocolResponse)executor.Execute(request);
							break;

						case AuthenticationRequest.Identifier:
							request = ProtocolBodyDeserializer.Deserialize<AuthenticationRequest>(requestObject.Body, protocolVersion);
							protocolResponse = (IProtocolResponse)executor.Execute(request);
							break;

						case ListDirectoryClientRequest.Identifier:
							request = ProtocolBodyDeserializer.Deserialize<ListDirectoryClientRequest>(requestObject.Body, protocolVersion);
							protocolResponse = (IProtocolResponse)executor.Execute(request);
							break;
					}

					string serializedBody = ProtocolBodySerializer.Serialize(protocolResponse, protocolVersion);
					string serializedResponse = SerializeHelper.SerializeRequestString(protocolResponse.GetIdentifier(protocolVersion), serializedBody, connectionState.AgreedHeaders, protocolVersion);

					Send(state, serializedResponse);
				}
			}
			else
			{
				// Not all data received. Get more.
				handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
					new AsyncCallback(OnDataReceived), state);
			}
		}

		private static void Send(StateObject state, String data)
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
					handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
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
			public const int BufferSize = 1024;

			public Socket WorkSocket { get; set; }

			public byte[] Buffer { get; } = new byte[BufferSize];

			public StringBuilder StringBuilder { get; } = new StringBuilder();

			public IConnectionState ProtocolState { get; set; }
		}
	}
}
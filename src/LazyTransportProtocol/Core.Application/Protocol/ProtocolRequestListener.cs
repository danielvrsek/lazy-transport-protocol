using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Services;
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

		private readonly IRequestExecutor _requestExecutor;

		private readonly
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
				// Set the event to nonsignaled state.
				allDone.Reset();

				// Start an asynchronous socket to listen for connections.
				Console.WriteLine("Waiting for a connection...");
				listener.BeginAccept(
					new AsyncCallback(AcceptCallback),
					listener);

				// Wait until a connection is made before continuing.
				allDone.WaitOne();
			}
		}

		public static void AcceptCallback(IAsyncResult ar)
		{
			// Signal the main thread to continue.
			allDone.Set();

			// Get the socket that handles the client request.
			Socket listener = (Socket)ar.AsyncState;
			Socket handler = listener.EndAccept(ar);

			// Create the state object.
			StateObject state = new StateObject
			{
				WorkSocket = handler
			};

			handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
				new AsyncCallback(ReadCallback), state);
		}

		public static void ReadCallback(IAsyncResult ar)
		{
			String content = String.Empty;

			// Retrieve the state object and the handler socket
			// from the asynchronous state object.
			StateObject state = (StateObject)ar.AsyncState;
			Socket handler = state.WorkSocket;

			// Read data from the client socket.
			int bytesRead = handler.EndReceive(ar);

			if (bytesRead > 0)
			{
				// There  might be more data, so store the data received so far.
				state.StringBuilder.Append(Encoding.ASCII.GetString(
					state.Buffer, 0, bytesRead));

				// Check for end-of-file tag. If it is not there, read
				// more data.
				content = state.StringBuilder.ToString();
				if (content.IndexOf("<EOF>") > -1)
				{
					// All the data has been read from the
					// client. Display it on the console.
					Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
						content.Length, content);
					// Echo the data back to the client.
					IRequest<IResponse> req = null;
					new ProtocolRequestExecutor().Execute(req);

					Send(handler, content);
				}
				else
				{
					// Not all data received. Get more.
					handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
					new AsyncCallback(ReadCallback), state);
				}
			}
		}

		private static void Send(Socket handler, String data)
		{
			// Convert the string data to byte data using ASCII encoding.
			byte[] byteData = Encoding.ASCII.GetBytes(data);

			// Begin sending the data to the remote device.
			handler.BeginSend(byteData, 0, byteData.Length, 0,
				new AsyncCallback(SendCallback), handler);
		}

		private static void SendCallback(IAsyncResult ar)
		{
			try
			{
				// Retrieve the socket from the state object.
				Socket handler = (Socket)ar.AsyncState;

				// Complete sending the data to the remote device.
				int bytesSent = handler.EndSend(ar);
				Console.WriteLine("Sent {0} bytes to client.", bytesSent);

				handler.Shutdown(SocketShutdown.Both);
				handler.Close();
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
		}
	}
}
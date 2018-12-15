
using LazyTransportProtocol.Core.Application.Transport.Extensions;
using LazyTransportProtocol.Core.Application.Transport.Infrastructure;
using LazyTransportProtocol.Core.Application.Transport.Requests;
using LazyTransportProtocol.Core.Application.Transport.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Transport.Handlers
{
	public class ListenToIncommingDataRequestHandler : IRequestHandler<ListenToIncommingDataRequest, ListenToIncommingDataResponse>
	{
		private ClientConnected onClientConnectedCallback;
		private DataReceived onAllDataReceivedCallback;
		private ErrorOccured onErrorOccuredCallback;

		private static ManualResetEvent allDone = new ManualResetEvent(false);

		public ListenToIncommingDataResponse GetResponse(ListenToIncommingDataRequest request)
		{
			var response = new ListenToIncommingDataResponse();

			onClientConnectedCallback = response.OnClientConnected;
			onAllDataReceivedCallback = response.OnDataReceived;
			onErrorOccuredCallback = response.OnErrorOccured;

			new Thread(() => Listen(request.IPAddress, request.Port)).Start();

			return response;
		}

		public Task<ListenToIncommingDataResponse> GetResponseAsync(ListenToIncommingDataRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public void Listen(IPAddress ipAddress, int port)
		{
			try
			{
				IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

				Socket listener = new Socket(ipAddress.AddressFamily,
					SocketType.Stream, ProtocolType.Tcp);

				listener.Bind(localEndPoint);
				listener.Listen(100);

				while (true)
				{
					allDone.Reset();

					listener.BeginAccept(new AsyncCallback(OnClientConnected), listener);

					allDone.WaitOne();
				}
			}
			catch (Exception e)
			{
				onErrorOccuredCallback(new ErrorContext
				{
					Exception = e
				});
			}
		}

		public void OnClientConnected(IAsyncResult ar)
		{
			allDone.Set();
			Socket listener = (Socket)ar.AsyncState;

			try
			{
				Socket handler = listener.EndAccept(ar);

				StateObject state = new StateObject();
				state.WorkSocket = handler;
				state.Connection = new SocketClientConnection(
					(d) => Send(state, d),
					() => Disconnect(handler));

				onClientConnectedCallback(state.Connection);

				handler.BeginReceive(state.Buffer, 0, state.Buffer.Length, 0, new AsyncCallback(OnDataReceived), state);
			}
			catch (Exception e)
			{
				onErrorOccuredCallback(new ErrorContext
				{
					Exception = e
				});
			}
		}


		public void OnDataReceived(IAsyncResult ar)
		{
			StateObject state = (StateObject)ar.AsyncState;

			try
			{
				Socket handler = state.WorkSocket;

				if (handler.EndReceive(ar, out SocketError error) > 1)
				{
					if (error != SocketError.Success)
					{
						onErrorOccuredCallback(new ErrorContext
						{
							SocketError = error
						});

						Disconnect(handler);
						return;
					}

					int dataLength = BitConverter.ToInt32(state.Buffer, 0);
					byte[] dataBuffer = new byte[dataLength];
					int received = 0;

					do
					{
						received = received + handler.Receive(dataBuffer, received, dataBuffer.Length - received, 0);
					}
					while (received != dataLength);

					onAllDataReceivedCallback(state.Connection, dataBuffer);
				}
				else
				{
					Disconnect(handler);
				}
			}
			catch (Exception e)
			{
				onErrorOccuredCallback(new ErrorContext
				{
					Exception = e
				});

				Disconnect(state.WorkSocket);
			}
		}

		private void Send(StateObject state, byte[] data)
		{
			try
			{
				byte[] dataLength = BitConverter.GetBytes(data.Length);
				byte[] transportData = dataLength.Append(data);

				state.WorkSocket.BeginSend(transportData, 0, transportData.Length, 0, new AsyncCallback(OnSendCompleted), state);
			}
			catch (Exception e)
			{
				onErrorOccuredCallback(new ErrorContext
				{
					Exception = e
				});

				Disconnect(state.WorkSocket);
			}
		}

		private void OnSendCompleted(IAsyncResult ar)
		{
			StateObject state = (StateObject)ar.AsyncState;

			try
			{
				Socket handler = state.WorkSocket;
				int bytesSent = handler.EndSend(ar);

				state.Buffer = new byte[4];
				handler.BeginReceive(state.Buffer, 0, state.Buffer.Length, 0, new AsyncCallback(OnDataReceived), state);
			}
			catch (Exception e)
			{
				onErrorOccuredCallback(new ErrorContext
				{
					Exception = e
				});

				Disconnect(state.WorkSocket);
			}
		}

		private void Disconnect(Socket socket)
		{
			try
			{
				socket.Shutdown(SocketShutdown.Both);
				socket.Close();
			}
			catch { }
		}

		public class StateObject
		{
			public SocketClientConnection Connection { get; set; }

			public byte[] Buffer { get; set; } = new byte[4];

			public Socket WorkSocket { get; set; }
		}
	}
}
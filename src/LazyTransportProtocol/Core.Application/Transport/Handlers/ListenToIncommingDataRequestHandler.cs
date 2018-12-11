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
		private static ManualResetEvent allDone = new ManualResetEvent(false);
		private static byte[] EndOfMessage => Encoding.UTF8.GetBytes("</>");

		public ListenToIncommingDataResponse GetResponse(ListenToIncommingDataRequest request)
		{
			var response = new ListenToIncommingDataResponse();

			new Thread(() => Listen(request.IPAddress, request.Port, request.BufferSize, response.OnDataReceived, response.OnErrorOccured)).Start();

			return response;
		}

		public Task<ListenToIncommingDataResponse> GetResponseAsync(ListenToIncommingDataRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public static void Listen(IPAddress ipAddress, int port, int bufferSize, DataReceived onAllDataReceivedCallback, ErrorOccured onErrorOccuredCallback)
		{
			try
			{
				IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

				Socket listener = new Socket(ipAddress.AddressFamily,
					SocketType.Stream, ProtocolType.Tcp);

				listener.Bind(localEndPoint);
				listener.Listen(100);

				StateObject state = new StateObject
				{
					WorkSocket = listener,
					BufferSize = bufferSize,
					AllDataReceivedCallback = onAllDataReceivedCallback,
					ErrorOccuredCallback = onErrorOccuredCallback
				};

				while (true)
				{
					allDone.Reset();

					listener.BeginAccept(new AsyncCallback(OnClientConnected), state);

					allDone.WaitOne();
				}
			}
			catch (Exception e)
			{
				onErrorOccuredCallback(null, e);
			}
		}

		public static void OnClientConnected(IAsyncResult ar)
		{
			StateObject state = (StateObject)ar.AsyncState;
			allDone.Set();

			try
			{
				Socket listener = state.WorkSocket;
				Socket handler = listener.EndAccept(ar);
				state.WorkSocket = handler;
				state.Connection = new SocketClientConnection(
						(d) => Send(state, d),
						() => Disconnect(state));

				byte[] buffer = state.NewBuffer;
				handler.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(OnDataReceived), state);
			}
			catch (Exception e)
			{
				state.ErrorOccuredCallback(state.Connection, e);
			}
		}

		public static void OnDataReceived(IAsyncResult ar)
		{
			StateObject state = (StateObject)ar.AsyncState;

			try
			{
				Socket handler = state.WorkSocket;

				int bytesRead = handler.EndReceive(ar);

				ArraySegment<byte> segment = new ArraySegment<byte>(state.CurrentBuffer, 0, bytesRead);

				state.AddData(segment);

				if (segment.EndsWith(EndOfMessage))
				{
					int dataLength = state.DataLength - EndOfMessage.Length;
					byte[] data = new byte[dataLength];

					int index = 0;
					foreach (var seg in state.Data)
					{
						for (int i = 0; i < seg.Count && index < dataLength; i++, index++)
						{
							data[index] = seg[i];
						}
					}

					state.AllDataReceivedCallback(state.Connection, data);

					state.Reset();
				}
				else
				{
					// Not all data received. Get more.
					byte[] buffer = state.NewBuffer;
					handler.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(OnDataReceived), state);
				}
			}
			catch (Exception e)
			{
				state.ErrorOccuredCallback(state.Connection, e);
			}
		}

		private static void Send(StateObject state, byte[] data)
		{
			byte[] transportData = data.Append(EndOfMessage);

			state.WorkSocket.BeginSend(transportData, 0, transportData.Length, 0, new AsyncCallback(OnSendCompleted), state);
		}

		private static void OnSendCompleted(IAsyncResult ar)
		{
			StateObject state = (StateObject)ar.AsyncState;

			try
			{
				Socket handler = state.WorkSocket;
				int bytesSent = handler.EndSend(ar);

				/*byte[] buffer = state.NewBuffer;
				handler.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(OnDataReceived), state);*/
			}
			catch (Exception e)
			{
				state.ErrorOccuredCallback(state.Connection, e);
			}
		}

		private static void Disconnect(StateObject state)
		{
			state.WorkSocket.Shutdown(SocketShutdown.Both);
			state.WorkSocket.Close();
		}

		public class StateObject
		{
			private int currentBufferIndex = -1;
			private List<byte[]> buffers = new List<byte[]>();
			private List<ArraySegment<byte>> data = new List<ArraySegment<byte>>();

			public SocketClientConnection Connection { get; set; }

			public int BufferSize { get; set; } = 2048;

			public byte[] CurrentBuffer => buffers[currentBufferIndex];

			public byte[] NewBuffer => GetNewBuffer(BufferSize);

			public int DataLength { get; private set; } = 0;

			public IReadOnlyList<ArraySegment<byte>> Data => data;

			public Socket WorkSocket { get; set; }

			public DataReceived AllDataReceivedCallback { get; set; }

			public ErrorOccured ErrorOccuredCallback { get; set; }

			public void AddData(ArraySegment<byte> segment)
			{
				data.Add(segment);
				DataLength += segment.Count;
			}

			public void Reset()
			{
				currentBufferIndex = -1;
				DataLength = 0;
			}

			private byte[] GetNewBuffer(int size)
			{
				byte[] buffer;

				if (currentBufferIndex + 1 >= buffers.Count)
				{
					buffer = new byte[BufferSize];
					buffers.Add(buffer);
				}
				else
				{
					buffer = buffers[currentBufferIndex + 1];
				}

				currentBufferIndex++;
				return buffer;
			}
		}
	}
}
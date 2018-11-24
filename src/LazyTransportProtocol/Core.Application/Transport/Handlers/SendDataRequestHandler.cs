using LazyTransportProtocol.Core.Application.Transport.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Transport.Requests;
using LazyTransportProtocol.Core.Application.Transport.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Transport.Handlers
{
	public class SendDataRequestHandler : ITransportRequestHandler<SendDataRequest, SendDataResponse>
	{
		public SendDataResponse GetResponse(SendDataRequest request)
		{
			Socket socket = request.Sender;
			int recvd = socket.Send(request.Data);

			IList<ArraySegment<byte>> bufferList = Receive(request.Sender);

			int responseSize = 0;

			foreach (var seg in bufferList)
			{
				responseSize += seg.Count;
			}

			byte[] data = new byte[responseSize];
			int index = 0;

			foreach (var seg in bufferList)
			{
				for (int i = 0; i < seg.Count; i++, index++)
				{
					data[index] = seg[i];
				}
			}

			return new SendDataResponse
			{
				ResponseData = data
			};
		}

		public IList<ArraySegment<byte>> Receive(Socket socket)
		{
			IList<ArraySegment<byte>> buffers = new List<ArraySegment<byte>>();

			byte[] buffer;

			do
			{
				buffer = new byte[2048];
				int receivedBytes = socket.Receive(buffer);

				ArraySegment<byte> segment = new ArraySegment<byte>(buffer, 0, receivedBytes);
				buffers.Add(segment);
			}
			while (socket.Available > 0);

			return buffers;
		}
	}
}
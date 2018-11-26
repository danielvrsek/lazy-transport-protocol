using LazyTransportProtocol.Core.Application.Protocol.Extensions;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Transport.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Transport.Requests;
using LazyTransportProtocol.Core.Application.Transport.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions.Common;
using LazyTransportProtocol.Core.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
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

		public Task<SendDataResponse> GetResponseAsync(SendDataRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public IList<ArraySegment<byte>> Receive(Socket socket)
		{
			IEncoder encoder = new ProtocolEncoder();
			byte[] eof = encoder.Encode("<EOF>");
			IList<ArraySegment<byte>> buffers = new List<ArraySegment<byte>>();

			byte[] buffer = null;
			ArraySegment<byte> segment = null;
			int noDataReceivedCount = 0;
			
			do
			{
				if (buffer == null)
				{
					buffer = new byte[2048];
				}

				int receivedBytes = socket.Receive(buffer);

				if (receivedBytes > 0)
				{
					segment = new ArraySegment<byte>(buffer, 0, receivedBytes);
					buffers.Add(segment);
					noDataReceivedCount = 0;

					buffer = null;
				}
				else if (noDataReceivedCount > 10)
				{
					throw new CustomException("Server timeout.");
				}
				else
				{
					noDataReceivedCount++;
				}

			}
			while (!segment.EndsWith(eof));

			ArraySegment<byte> lastSegment = buffers[buffers.Count - 1];
			buffers[buffers.Count - 1] = lastSegment.Slice(0, lastSegment.Count - eof.Length);

			return buffers;
		}
	}
}
using LazyTransportProtocol.Core.Application.Transport.Requests;
using LazyTransportProtocol.Core.Application.Transport.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Transport.Handlers
{
	internal class SendDataRequestHandler : IRequestHandler<SendDataRequest, SendDataResponse>
	{
		public SendDataResponse GetResponse(SendDataRequest request)
		{
			ArraySegment<byte> dataLength = new ArraySegment<byte>(BitConverter.GetBytes(request.Data.Count));
			List<ArraySegment<byte>> transportData = new List<ArraySegment<byte>> { dataLength, request.Data };

			Socket socket = request.Sender;
			socket.Send(transportData);

			byte[] responseData = Receive(request.Sender);

			return new SendDataResponse
			{
				ResponseData = responseData
			};
		}

		public Task<SendDataResponse> GetResponseAsync(SendDataRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public byte[] Receive(Socket socket)
		{
			byte[] dataLengthBytes = new byte[4];
			socket.Receive(dataLengthBytes);

			int dataLength = BitConverter.ToInt32(dataLengthBytes, 0);

			byte[] data = new byte[dataLength];

			int received = socket.Receive(data);

			while (received != dataLength)
			{
				received = received + socket.Receive(data, received, data.Length - received, 0);
			}

			return data;
		}
	}
}
using LazyTransportProtocol.Core.Application.Transport.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Transport.Requests;
using LazyTransportProtocol.Core.Application.Transport.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
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
			socket.Send(request.Data);

			byte[] bytes = new byte[1024];
			int bytesRec = socket.Receive(bytes);

			byte[] msg = new byte[bytesRec];
			for (int i = 0; i < bytesRec; i++)
			{
				msg[i] = bytes[i];
			}

			return new SendDataResponse
			{
				ResponseData = msg
			};
		}
	}
}
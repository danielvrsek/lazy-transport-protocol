using LazyTransportProtocol.Core.Application.Transport.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Transport.Requests;
using LazyTransportProtocol.Core.Application.Transport.Responses;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Transport.Handlers
{
	public class EndConnectionRequestHandler : ITransportRequestHandler<EndConnectionRequest, EndConnectionResponse>
	{
		public EndConnectionResponse GetResponse(EndConnectionRequest request)
		{
			Socket socket = request.Sender;

			socket.Shutdown(SocketShutdown.Both);
			socket.Close();

			return new EndConnectionResponse();
		}
	}
}
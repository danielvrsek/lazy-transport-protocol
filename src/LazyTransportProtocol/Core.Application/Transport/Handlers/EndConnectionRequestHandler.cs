using LazyTransportProtocol.Core.Application.Transport.Requests;
using LazyTransportProtocol.Core.Application.Transport.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Transport.Handlers
{
	internal class EndConnectionRequestHandler : IRequestHandler<EndConnectionRequest, EndConnectionResponse>
	{
		public EndConnectionResponse GetResponse(EndConnectionRequest request)
		{
			Socket socket = request.Sender;

			socket.Shutdown(SocketShutdown.Both);
			socket.Close();

			return new EndConnectionResponse();
		}

		public Task<EndConnectionResponse> GetResponseAsync(EndConnectionRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
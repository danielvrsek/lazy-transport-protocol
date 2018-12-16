using LazyTransportProtocol.Core.Application.Transport.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using System.Net.Sockets;

namespace LazyTransportProtocol.Core.Application.Transport.Requests
{
	internal class EndConnectionRequest : IRequest<EndConnectionResponse>
	{
		public Socket Sender { get; set; }
	}
}
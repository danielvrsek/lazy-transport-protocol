using LazyTransportProtocol.Core.Application.Transport.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Transport.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions;
using System.Net.Sockets;

namespace LazyTransportProtocol.Core.Application.Transport.Requests
{
	public class EndConnectionRequest : ITransportRequest<EndConnectionResponse>
	{
		public Socket Sender { get; set; }
	}
}
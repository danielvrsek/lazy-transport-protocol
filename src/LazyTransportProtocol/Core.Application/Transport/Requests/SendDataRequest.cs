using LazyTransportProtocol.Core.Application.Transport.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using System.Net.Sockets;

namespace LazyTransportProtocol.Core.Application.Transport.Requests
{
	internal class SendDataRequest : IRequest<SendDataResponse>
	{
		public byte[] Data { get; set; }

		public Socket Sender { get; set; }
	}
}
using LazyTransportProtocol.Core.Application.Transport.Abstractions.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions;

namespace LazyTransportProtocol.Core.Application.Transport.Responses
{
	public class SendDataResponse : ITransportResponse
	{
		public byte[] ResponseData { get; set; }

		public IConnection ConnectionContext { get; set; }
	}
}
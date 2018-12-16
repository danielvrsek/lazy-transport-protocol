using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;

namespace LazyTransportProtocol.Core.Application.Transport.Responses
{
	internal class SendDataResponse : IResponse
	{
		public byte[] ResponseData { get; set; }

		public IServerConnection ConnectionContext { get; set; }
	}
}
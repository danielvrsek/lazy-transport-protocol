using LazyTransportProtocol.Core.Domain.Abstractions;

namespace LazyTransportProtocol.Core.Application.Transport.DataModel
{
	public class BeginConnectDto
	{
		public IConnection ConnectionContext { get; set; }
	}
}
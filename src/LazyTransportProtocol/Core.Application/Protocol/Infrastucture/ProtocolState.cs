using LazyTransportProtocol.Core.Application.Protocol.Model;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Domain.Abstractions;

namespace LazyTransportProtocol.Core.Application.Infrastructure
{
	public class ProtocolState : IConnectionState
	{
		public ProtocolVersion ProtocolVersion { get; set; }

		public string Separator { get; set; }

		public AuthenticatedClient AuthenticatedClient { get; set; }
	}
}
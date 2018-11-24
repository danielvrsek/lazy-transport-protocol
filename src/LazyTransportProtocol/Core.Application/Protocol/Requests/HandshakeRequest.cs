using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Infrastructure;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;

namespace LazyTransportProtocol.Core.Application.Protocol.Requests
{
	public class HandshakeRequest : IProtocolRequest<AcknowledgementResponse>
	{
		public const string Identifier = "HANDSHAKE";

		public ProtocolVersion ProtocolVersion { get; set; }

		public int BufferSize { get; set; }

		public string Separator { get; set; }

		public IAuthenticationContext AuthenticationContext { get; set; }

		public string GetIdentifier(ProtocolVersion protocolVersion)
		{
			return Identifier;
		}
	}
}
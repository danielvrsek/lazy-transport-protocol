using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Infrastructure;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.Metadata;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;

namespace LazyTransportProtocol.Core.Application.Protocol.Requests
{
	public class CreateUserRequest : IProtocolRequest<AcknowledgementResponse>
	{
		public const string Identifier = "CREATEUSER";

		public string Username { get; set; }

		public string Password { get; set; }

		public IAuthenticationContext AuthenticationContext { get; set; }

		public string GetIdentifier(ProtocolVersion protocolVersion)
		{
			return Identifier;
		}
	}
}
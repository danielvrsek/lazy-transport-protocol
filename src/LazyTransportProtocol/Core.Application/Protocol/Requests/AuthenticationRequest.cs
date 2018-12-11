using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Infrastructure;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;

namespace LazyTransportProtocol.Core.Application.Protocol.Requests
{
	public class AuthenticationRequest : IProtocolRequest<AuthenticationResponse>
	{
		public const string Identifier = "AUTHENTICATE";

		public string Username { get; set; }

		public string Password { get; set; }

		public string GetIdentifier()
		{
			return Identifier;
		}
	}
}
using LazyTransportProtocol.Core.Application.Protocol.Requests.Abstractions;
using LazyTransportProtocol.Core.Application.Protocol.Responses;

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

	public class AuthenticationRequest2
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
using LazyTransportProtocol.Core.Application.Protocol.Requests.Abstractions;
using LazyTransportProtocol.Core.Application.Protocol.Responses;

namespace LazyTransportProtocol.Core.Application.Protocol.Requests
{
	public class CreateUserRequest : IAuthenticatedRequest<AcknowledgementResponse>
	{
		public const string Identifier = "CREATEUSER";

		public string Username { get; set; }

		public string Password { get; set; }

		public string AuthenticationToken { get; set; }

		public string GetIdentifier()
		{
			return Identifier;
		}
	}
}
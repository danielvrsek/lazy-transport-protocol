using LazyTransportProtocol.Core.Application.Protocol.Requests.Abstractions;
using LazyTransportProtocol.Core.Application.Protocol.Responses;

namespace LazyTransportProtocol.Core.Application.Protocol.Requests
{
	public class DeleteUserRequest : IAuthenticatedRequest<AcknowledgementResponse>
	{
		public const string Identifier = "USERDEL";

		public string Username { get; set; }

		public string AuthenticationToken { get; set; }

		public string GetIdentifier()
		{
			return Identifier;
		}
	}
}
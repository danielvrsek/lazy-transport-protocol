using LazyTransportProtocol.Core.Application.Protocol.Requests.Abstractions;
using LazyTransportProtocol.Core.Application.Protocol.Responses;

namespace LazyTransportProtocol.Core.Application.Protocol.Requests
{
	public class CreateDirectoryRequest : IAuthenticatedRequest<AcknowledgementResponse>
	{
		public const string Identifier = "MKDIR";

		public string Path { get; set; }

		public string AuthenticationToken { get; set; }

		public string GetIdentifier()
		{
			return Identifier;
		}
	}
}
using LazyTransportProtocol.Core.Application.Protocol.Requests.Abstractions;
using LazyTransportProtocol.Core.Application.Protocol.Responses;

namespace LazyTransportProtocol.Core.Application.Protocol.Requests
{
	public class UploadFileRequest : IAuthenticatedRequest<AcknowledgementResponse>
	{
		public const string Identifier = "UPLOADFILE";

		public string Path { get; set; }

		public byte[] Data { get; set; }

		public int Offset { get; set; }

		public string AuthenticationToken { get; set; }

		public string GetIdentifier()
		{
			return Identifier;
		}
	}
}
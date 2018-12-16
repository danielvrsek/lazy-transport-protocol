using LazyTransportProtocol.Core.Application.Protocol.Requests.Abstractions;
using LazyTransportProtocol.Core.Application.Protocol.Responses;

namespace LazyTransportProtocol.Core.Application.Protocol.Requests
{
	//[Authorize(Resource = Resources.File, Action = Actions.Read)]
	public class DownloadFileRequest : IAuthenticatedRequest<DownloadFileResponse>
	{
		public const string Identifier = "DOWNFILE";

		public string Filepath { get; set; }

		public int Offset { get; set; }

		public int Count { get; set; }

		public string AuthenticationToken { get; set; }

		public string GetIdentifier()
		{
			return Identifier;
		}
	}
}
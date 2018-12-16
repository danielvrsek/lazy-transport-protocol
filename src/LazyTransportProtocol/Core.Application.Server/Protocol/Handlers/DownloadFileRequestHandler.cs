using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Server.Protocol.Handlers.Abstraction;
using LazyTransportProtocol.Core.Application.Server.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Server.Protocol.Handlers
{
	internal class DownloadFileRequestHandler : IProtocolRequestHandler<DownloadFileRequest, DownloadFileResponse>
	{
		public DownloadFileResponse GetResponse(DownloadFileRequest request)
		{
			AuthorizationService service = new AuthorizationService();

			byte[] data = IOService.ReadFile(request.Filepath, request.Offset, request.Count);

			return new DownloadFileResponse
			{
				Data = data,
				HasNext = data.Length == request.Count
			};
		}

		public Task<DownloadFileResponse> GetResponseAsync(DownloadFileRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
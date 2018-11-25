using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Protocol.Handlers
{
	public class DownloadFileRequestHandler : IProtocolRequestHandler<DownloadFileRequest, DownloadFileResponse>
	{
		public DownloadFileResponse GetResponse(DownloadFileRequest request)
		{
			Span<byte> data = IOService.ReadFile(request.Filepath, request.Offset, request.Count);

			return new DownloadFileResponse
			{
				Data = data.ToArray(),
				HasNext = data.Length == request.Count
			};
		}

		public Task<DownloadFileResponse> GetResponseAsync(DownloadFileRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}

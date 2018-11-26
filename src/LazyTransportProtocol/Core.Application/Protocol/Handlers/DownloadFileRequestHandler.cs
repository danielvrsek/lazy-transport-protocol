using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Domain.Exceptions.Authorization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Protocol.Handlers
{
	public class DownloadFileRequestHandler : IProtocolRequestHandler<DownloadFileRequest, DownloadFileResponse>
	{
		public DownloadFileResponse GetResponse(DownloadFileRequest request)
		{
			AuthorizationService service = new AuthorizationService();
			if (!service.HasAccessToDirectory(request.AuthenticationContext, Path.GetDirectoryName(request.Filepath)))
			{
				throw new AuthorizationException();
			}

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

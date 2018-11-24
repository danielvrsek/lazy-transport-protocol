using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Handlers
{
	public class DownloadFileRequestHandler : IProtocolRequestHandler<DownloadFileRequest, DownloadFileResponse>
	{
		public DownloadFileResponse GetResponse(DownloadFileRequest request)
		{
			IOService service = new IOService();
			byte[] data = service.ReadFile(request.Filepath, request.Offset, request.Count);

			return new DownloadFileResponse
			{
				Data = data,
				HasNext = data.Length == request.Count
			};
		}
	}
}

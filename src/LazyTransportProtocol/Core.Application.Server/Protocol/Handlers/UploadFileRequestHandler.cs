using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Server.Protocol.Handlers.Abstraction;
using LazyTransportProtocol.Core.Application.Server.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Server.Protocol.Handlers
{
	internal class UploadFileRequestHandler : IProtocolRequestHandler<UploadFileRequest, AcknowledgementResponse>
	{
		public AcknowledgementResponse GetResponse(UploadFileRequest request)
		{
			int code;

			try
			{
				if (request.Offset == 0 && IOService.FileExists(request.Path))
				{
					IOService.DeleteFile(request.Path);
				}

				IOService.AppendFile(request.Path, request.Data, request.Offset);

				code = 200;
			}
			catch
			{
				code = 400;
			}

			return new AcknowledgementResponse
			{
				Code = code
			};
		}

		public Task<AcknowledgementResponse> GetResponseAsync(UploadFileRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
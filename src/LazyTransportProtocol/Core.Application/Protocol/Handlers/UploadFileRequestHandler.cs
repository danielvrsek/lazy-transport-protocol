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
	public class UploadFileRequestHandler : IProtocolRequestHandler<UploadFileRequest, AcknowledgementResponse>
	{
		public AcknowledgementResponse GetResponse(UploadFileRequest request)
		{
			int code;

			try
			{
				IOService.AppendFile(request.Path, request.Data);
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

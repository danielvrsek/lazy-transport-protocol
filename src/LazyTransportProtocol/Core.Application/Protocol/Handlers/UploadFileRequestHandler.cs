using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Handlers
{
	public class UploadFileRequestHandler : IProtocolRequestHandler<UploadFileRequest, AcknowledgementResponse>
	{
		public AcknowledgementResponse GetResponse(UploadFileRequest request)
		{
			IOService service = new IOService();

			int code;

			try
			{
				service.AppendFile(request.Path, request.Data);
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
	}
}

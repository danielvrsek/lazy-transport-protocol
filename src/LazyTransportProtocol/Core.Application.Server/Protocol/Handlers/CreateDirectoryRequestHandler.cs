using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Server.Protocol.Handlers.Abstraction;
using LazyTransportProtocol.Core.Application.Server.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Server.Protocol.Handlers
{
	public class CreateDirectoryRequestHandler : IProtocolRequestHandler<CreateDirectoryRequest, AcknowledgementResponse>
	{
		public AcknowledgementResponse GetResponse(CreateDirectoryRequest request)
		{
			AuthorizationService authorizationService = new AuthorizationService();

			int code = 200;

			try
			{
				IOService.CreateDirectory(request.Path);
			}
			catch (ArgumentException)
			{
				code = 500;
			}
			catch
			{
				code = 500;
			}

			return new AcknowledgementResponse
			{
				Code = code
			};
		}

		public Task<AcknowledgementResponse> GetResponseAsync(CreateDirectoryRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
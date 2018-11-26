using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Domain.Exceptions;
using LazyTransportProtocol.Core.Domain.Exceptions.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Protocol.Handlers
{
	public class CreateDirectoryRequestHandler : IProtocolRequestHandler<CreateDirectoryRequest, AcknowledgementResponse>
	{
		public AcknowledgementResponse GetResponse(CreateDirectoryRequest request)
		{
			AuthorizationService authorizationService = new AuthorizationService();
			if (!authorizationService.HasAccessToDirectory(request.AuthenticationContext, request.Path))
			{
				throw new AuthorizationException();
			}

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

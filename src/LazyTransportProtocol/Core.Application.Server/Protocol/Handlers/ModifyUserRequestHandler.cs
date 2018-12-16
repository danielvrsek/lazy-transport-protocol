using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Server.Protocol.Handlers.Abstraction;
using LazyTransportProtocol.Core.Application.Server.Services;
using LazyTransportProtocol.Core.Application.Server.Services.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Server.Protocol.Handlers
{
	internal class ModifyUserRequestHandler : IProtocolRequestHandler<ModifyUserRequest, AcknowledgementResponse>
	{
		public AcknowledgementResponse GetResponse(ModifyUserRequest request)
		{
			IUserService userService = new UserService();

			userService.Modify(request.Username, request.Password);

			return new AcknowledgementResponse
			{
				Code = 200
			};
		}

		public Task<AcknowledgementResponse> GetResponseAsync(ModifyUserRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
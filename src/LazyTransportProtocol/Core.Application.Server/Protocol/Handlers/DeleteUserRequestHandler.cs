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
	public class DeleteUserRequestHandler : IProtocolRequestHandler<DeleteUserRequest, AcknowledgementResponse>
	{
		public AcknowledgementResponse GetResponse(DeleteUserRequest request)
		{
			IUserService userService = new UserService();

			userService.Delete(request.Username);

			return new AcknowledgementResponse
			{
				Code = 200
			};
		}

		public Task<AcknowledgementResponse> GetResponseAsync(DeleteUserRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
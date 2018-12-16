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
	public class CreateUserRequestHandler : IProtocolRequestHandler<CreateUserRequest, AcknowledgementResponse>
	{
		public AcknowledgementResponse GetResponse(CreateUserRequest request)
		{
			IUserService userService = new UserService();
			bool isSuccessful = true;

			try
			{
				userService.CreateNew(request.Username, request.Password);
			}
			catch
			{
				isSuccessful = false;
			}

			return new AcknowledgementResponse
			{
				Code = isSuccessful ? 200 : 400
			};
		}

		public Task<AcknowledgementResponse> GetResponseAsync(CreateUserRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
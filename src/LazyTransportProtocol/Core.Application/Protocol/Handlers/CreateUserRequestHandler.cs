using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Services;
using LazyTransportProtocol.Core.Application.Protocol.Model;
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
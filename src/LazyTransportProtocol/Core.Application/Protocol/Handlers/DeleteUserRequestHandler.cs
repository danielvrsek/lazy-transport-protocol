using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Services;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Handlers
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
	}
}

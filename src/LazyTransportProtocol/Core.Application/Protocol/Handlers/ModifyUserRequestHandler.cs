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
	public class ModifyUserRequestHandler : IProtocolRequestHandler<ModifyUserRequest, AcknowledgementResponse>
	{
		public AcknowledgementResponse GetResponse(ModifyUserRequest request)
		{
			IUserService userService = new UserService();

			userService.Modify(new UserSecret
			{
				Username = request.Username,
				Password = request.Password
			});

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

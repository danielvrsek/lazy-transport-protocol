using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Handlers
{
	public class CreateUserRequestHandler : IProtocolRequestHandler<CreateUserRequest, AcknowledgementResponse>
	{
		public AcknowledgementResponse GetResponse(CreateUserRequest request)
		{
			throw new NotImplementedException();
		}
	}
}
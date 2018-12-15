using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Infrastructure;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests
{
	internal interface IAuthenticatedRequest<out TResponse> : IProtocolRequest<TResponse>
		where TResponse : IProtocolResponse
	{
		IAuthenticationContext AuthenticationContext { get; set; }

		string AuthenticationToken { get; }
	}
}

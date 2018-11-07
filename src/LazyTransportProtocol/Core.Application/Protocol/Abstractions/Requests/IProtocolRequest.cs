using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests
{
	public interface IProtocolRequest<TResponse> : IRequest<TResponse>
		where TResponse : class, IProtocolResponse
	{
	}
}
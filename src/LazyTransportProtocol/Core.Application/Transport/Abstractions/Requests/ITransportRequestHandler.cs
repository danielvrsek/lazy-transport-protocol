using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Transport.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Transport.Abstractions.Requests
{
	public interface ITransportRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
		where TRequest : ITransportRequest<TResponse>
		where TResponse : class, ITransportResponse
	{
	}
}
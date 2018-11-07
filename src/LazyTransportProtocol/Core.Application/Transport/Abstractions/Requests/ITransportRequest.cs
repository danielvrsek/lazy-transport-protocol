using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Transport.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Transport.Abstractions.Requests
{
	public interface ITransportRequest<TResponse> : IRequest<TResponse>
		where TResponse : class, ITransportResponse
	{
	}
}
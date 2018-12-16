using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Domain.Abstractions.Requests
{
	/// <summary>
	/// Request handler
	/// </summary>
	public interface IRequestHandler<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
		where TResponse : IResponse
	{
		TResponse GetResponse(TRequest request);

		Task<TResponse> GetResponseAsync(TRequest request, CancellationToken cancellationToken);
	}
}
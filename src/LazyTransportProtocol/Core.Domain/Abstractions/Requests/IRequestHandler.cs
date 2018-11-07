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
	public interface IRequestHandler<in TRequest, out TResponse>
		where TRequest : IRequest<TResponse>
		where TResponse : class, IResponse
	{
		TResponse GetResponse(TRequest request);
	}
}
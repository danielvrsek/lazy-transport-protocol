using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;

namespace LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests
{
	/// <summary>
	/// Interface for all protocol request handlers to implement
	/// </summary>
	/// <typeparam name="TRequest">Type of the request which this handler handles</typeparam>
	/// <typeparam name="TResponse">Type of the response which this handler returns</typeparam>
	public interface IProtocolRequestHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
		where TRequest : IProtocolRequest<TResponse>
		where TResponse : class, IProtocolResponse, new()
	{
	}
}
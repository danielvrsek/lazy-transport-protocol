using LazyTransportProtocol.Core.Application.Protocol.Requests.Abstractions;
using LazyTransportProtocol.Core.Application.Protocol.Responses.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;

namespace LazyTransportProtocol.Core.Application.Server.Protocol.Handlers.Abstraction
{
	/// <summary>
	/// Interface for all protocol request handlers to implement
	/// </summary>
	/// <typeparam name="TRequest">Type of the request which this handler handles</typeparam>
	/// <typeparam name="TResponse">Type of the response which this handler returns</typeparam>
	public interface IProtocolRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
		where TRequest : IProtocolRequest<TResponse>
		where TResponse : IProtocolResponse, new()
	{
	}
}
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;

namespace LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests
{
	public interface IProtocolRequestHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
		where TRequest : IProtocolRequest<TResponse>
		where TResponse : class, IProtocolResponse, new()
	{
	}
}
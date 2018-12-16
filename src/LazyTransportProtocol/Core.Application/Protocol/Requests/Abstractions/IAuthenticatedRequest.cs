using LazyTransportProtocol.Core.Application.Protocol.Responses.Abstractions;

namespace LazyTransportProtocol.Core.Application.Protocol.Requests.Abstractions
{
	public interface IAuthenticatedRequest<out TResponse> : IProtocolRequest<TResponse>
		where TResponse : IProtocolResponse
	{
		string AuthenticationToken { get; }
	}
}
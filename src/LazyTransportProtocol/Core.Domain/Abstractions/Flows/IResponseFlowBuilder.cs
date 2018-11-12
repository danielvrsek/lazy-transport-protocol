using LazyTransportProtocol.Core.Domain.Abstractions.Responses;

namespace LazyTransportProtocol.Core.Domain.Abstractions.Flows
{
	public interface IResponseFlowBuilder<TResponse>
		where TResponse : IResponse
	{
	}
}
using LazyTransportProtocol.Core.Domain.Abstractions.Pipeline;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Domain.Abstractions.Flows
{
	public interface IRequestFlowBuilder
	{
		IResponseFlowBuilder<TResponse> For<TResponse>()
			where TResponse : class, IResponse;

		IResponseFlowBuilder<TResponse> For<TRequest, TResponse>(IPipelineQueue<TRequest> pipelineQueue)
			where TRequest : IRequest<TResponse>
			where TResponse : class, IResponse;
	}
}
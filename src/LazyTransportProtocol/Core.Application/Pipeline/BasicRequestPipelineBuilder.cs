using LazyTransportProtocol.Core.Domain.Abstractions.Pipeline;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions.Validators;
using System;

namespace LazyTransportProtocol.Core.Application.Pipeline
{
	internal class BasicRequestPipelineBuilder<TRequest, TResponse> : IPipelineBuilder<TRequest>
		where TRequest : IRequest<TResponse>
		where TResponse : IResponse
	{
		private BasicPipelineQueue<TRequest> basicPipelineQueue = new BasicPipelineQueue<TRequest>();

		public IPipelineBuilder<TRequest> AddPipelineAction(Action<TRequest> pipelineAction)
		{
			basicPipelineQueue.AddToQueue((request) =>
			{
				pipelineAction(request);

				return request;
			});

			return this;
		}

		public IPipelineBuilder<TRequest> AddToQueue(Func<TRequest, TRequest> pipelineFunc)
		{
			basicPipelineQueue.AddToQueue(pipelineFunc);

			return this;
		}

		public IPipelineBuilder<TRequest> AddValidator(IPipelineValidator<TRequest> validator)
		{
			AddPipelineAction((request) => validator.Validate(request));

			return this;
		}

		public IPipelineBuilder<TRequest> OnException(Action<IPipelineExceptionContext<TRequest>> action)
		{
			basicPipelineQueue.OnError(action);

			return this;
		}

		public IPipelineQueue<TRequest> Build()
		{
			return basicPipelineQueue;
		}
	}
}
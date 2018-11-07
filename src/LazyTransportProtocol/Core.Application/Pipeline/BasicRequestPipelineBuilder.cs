using LazyTransportProtocol.Core.Domain.Abstractions.Pipeline;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions.Validators;

namespace LazyTransportProtocol.Core.Application.Pipeline
{
	public class BasicRequestPipelineBuilder<TRequest> : IRequestPipelineBuilder<TRequest>
		where TRequest : IRequest<IResponse>
	{
		private BasicPipelineQueue<TRequest> basicPipelineQueue = new BasicPipelineQueue<TRequest>();

		public IRequestPipelineBuilder<TRequest> AddPipelineAction(PipelineAction<TRequest> pipelineAction)
		{
			basicPipelineQueue.AddToQueue(pipelineAction);

			return this;
		}

		public IRequestPipelineBuilder<TRequest> AddValidator(IRequestValidator<TRequest> validator)
		{
			AddPipelineAction((request) => validator.Validate(request));

			return this;
		}

		public IPipelineQueue<TRequest> Build()
		{
			return basicPipelineQueue;
		}
	}
}
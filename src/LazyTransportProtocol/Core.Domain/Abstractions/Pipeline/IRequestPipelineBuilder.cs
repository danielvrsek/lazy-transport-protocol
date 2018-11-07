using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions.Validators;

namespace LazyTransportProtocol.Core.Domain.Abstractions.Pipeline
{
	public delegate void PipelineAction<in TRequest>(TRequest request)
		where TRequest : IRequest<IResponse>;

	public interface IRequestPipelineBuilder<TRequest>
		where TRequest : IRequest<IResponse>
	{
		IRequestPipelineBuilder<TRequest> AddPipelineAction(PipelineAction<TRequest> pipelineAction);

		IRequestPipelineBuilder<TRequest> AddValidator(IRequestValidator<TRequest> validator);

		IPipelineQueue<TRequest> Build();
	}
}
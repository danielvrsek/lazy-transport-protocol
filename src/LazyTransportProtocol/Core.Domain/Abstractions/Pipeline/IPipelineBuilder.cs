using LazyTransportProtocol.Core.Domain.Abstractions.Validators;
using System;

namespace LazyTransportProtocol.Core.Domain.Abstractions.Pipeline
{
	public interface IPipelineBuilder<T>
	{
		IPipelineBuilder<T> AddToQueue(Func<T, T> pipelineFunc);

		IPipelineBuilder<T> AddValidator(IPipelineValidator<T> validator);

		IPipelineBuilder<T> OnException(Action<IPipelineExceptionContext<T>> action);

		IPipelineQueue<T> Build();
	}
}
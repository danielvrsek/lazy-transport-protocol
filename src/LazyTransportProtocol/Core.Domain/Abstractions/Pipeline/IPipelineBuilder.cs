using LazyTransportProtocol.Core.Domain.Abstractions.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Domain.Abstractions.Pipeline
{
	public interface IPipelineBuilder<T>
	{
		IPipelineBuilder<T> AddPipelineAction(Action<T> pipelineAction);

		IPipelineBuilder<T> AddPipelineFunction(Func<T, T> pipelineFunc);

		IPipelineBuilder<T> OnException(Action<IPipelineExceptionContext<T>> action);

		IPipelineBuilder<T> AddValidator(IPipelineValidator<T> validator);

		IPipelineQueue<T> Build();
	}
}
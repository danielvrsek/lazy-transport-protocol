using LazyTransportProtocol.Core.Domain.Abstractions.Pipeline;
using System;

namespace LazyTransportProtocol.Core.Application.Pipeline
{
	internal class PipelineExceptionContext<T> : IPipelineExceptionContext<T>
	{
		public Exception Exception { get; set; }

		public T Request { get; set; }
	}
}
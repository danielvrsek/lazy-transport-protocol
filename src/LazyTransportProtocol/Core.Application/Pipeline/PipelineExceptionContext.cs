using LazyTransportProtocol.Core.Domain.Abstractions.Pipeline;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Pipeline
{
	public class PipelineExceptionContext<T> : IPipelineExceptionContext<T>
	{
		public Exception Exception { get; set; }

		public T Request { get; set; }
	}
}
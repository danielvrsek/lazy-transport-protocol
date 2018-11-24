using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Domain.Abstractions.Pipeline
{
	public interface IPipelineQueue<T>
	{
		T ExecutePipelineQueue(T request);
	}
}
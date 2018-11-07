using LazyTransportProtocol.Core.Domain.Abstractions.Pipeline;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using System.Collections.Generic;

namespace LazyTransportProtocol.Core.Application.Pipeline
{
	public class BasicPipelineQueue<TRequest> : IPipelineQueue<TRequest>
		where TRequest : IRequest<IResponse>
	{
		private object _lock = new object();

		private List<PipelineAction<TRequest>> _pipelineActions = new List<PipelineAction<TRequest>>();

		public void AddToQueue(PipelineAction<TRequest> pipelineAction)
		{
			lock (_lock)
			{
				_pipelineActions.Add(pipelineAction);
			}
		}

		public void ExecutePipelineQueue(TRequest request)
		{
			foreach (PipelineAction<TRequest> action in _pipelineActions)
			{
				action(request);
			}
		}
	}
}
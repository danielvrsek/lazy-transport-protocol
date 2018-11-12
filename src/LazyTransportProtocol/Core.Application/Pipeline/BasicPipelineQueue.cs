using LazyTransportProtocol.Core.Domain.Abstractions.Pipeline;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using System;
using System.Collections.Generic;

namespace LazyTransportProtocol.Core.Application.Pipeline
{
	public class BasicPipelineQueue<TRequest> : IPipelineQueue<TRequest>
		where TRequest : IRequest<IResponse>
	{
		private readonly object _lock = new object();

		private List<Action<TRequest>> _pipelineActions = new List<Action<TRequest>>();

		private Action<RequestExceptionContext<TRequest>> _onExceptionAction = null;

		public void AddToQueue(Action<TRequest> pipelineAction)
		{
			lock (_lock)
			{
				_pipelineActions.Add(pipelineAction);
			}
		}

		public void ExecutePipelineQueue(TRequest request)
		{
			try
			{
				foreach (Action<TRequest> action in _pipelineActions)
				{
					action(request);
				}
			}
			catch (Exception e)
			{
				_onExceptionAction?.Invoke(new RequestExceptionContext<TRequest>
				{
					Exception = e,
					Request = request
				});
				throw;
			}
		}

		public void OnError(Action<RequestExceptionContext<TRequest>> action)
		{
			_onExceptionAction = action;
		}
	}
}
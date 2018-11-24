using LazyTransportProtocol.Core.Domain.Abstractions.Pipeline;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using System;
using System.Collections.Generic;

namespace LazyTransportProtocol.Core.Application.Pipeline
{
	/// <summary>
	/// Default implemntation of the <see cref="IPipelineBuilder{T}"/>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class BasicPipelineQueue<T> : IPipelineQueue<T>
	{
		private readonly object _lock = new object();

		private List<Func<T, T>> _pipelineFuncs = new List<Func<T, T>>();

		private Action<PipelineExceptionContext<T>> _onExceptionAction = null;

		public void AddToQueue(Func<T, T> pipelineFunc)
		{
			lock (_lock)
			{
				_pipelineFuncs.Add(pipelineFunc);
			}
		}

		public T ExecutePipelineQueue(T request)
		{
			try
			{
				foreach (Func<T, T> func in _pipelineFuncs)
				{
					request = func(request);
				}

				return request;
			}
			catch (Exception e)
			{
				_onExceptionAction?.Invoke(new PipelineExceptionContext<T>
				{
					Exception = e,
					Request = request
				});
				throw;
			}
		}

		public void OnError(Action<PipelineExceptionContext<T>> action)
		{
			_onExceptionAction = action;
		}
	}
}
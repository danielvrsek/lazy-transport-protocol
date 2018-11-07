using LazyTransportProtocol.Core.Application.Pipeline;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Pipeline;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Abstractions
{
	public abstract class RequestExecutorBase : IRequestExecutor
	{
		public static object _lock = new object();

		protected static Dictionary<Type, RequestHandlerDelegate<IRequest<IResponse>, IResponse>> _requestHandlerDictionary = new Dictionary<Type, RequestHandlerDelegate<IRequest<IResponse>, IResponse>>();

		public static void Register<TRequest>(RequestHandlerDelegate<IRequest<IResponse>, IResponse> requestHandlerDelegate, IPipelineQueue<TRequest> pipelineQueue)
			where TRequest : IRequest<IResponse>
		{
			IRequestPipelineBuilder<TRequest> pipelineBuilder = new BasicRequestPipelineBuilder<TRequest>();

			lock (_lock)
			{
				_requestHandlerDictionary[typeof(TRequest)] = (request) =>
				{
					pipelineQueue.ExecutePipelineQueue((TRequest)request);

					return requestHandlerDelegate(request);
				};
			}
		}

		public static IRequestPipelineBuilder<TRequest> Register<TRequest>(RequestHandlerDelegate<TRequest, IResponse> requestHandlerDelegate)
			where TRequest : IRequest<IResponse>
		{
			return Register<TRequest, IResponse>(requestHandlerDelegate);
		}

		public static IRequestPipelineBuilder<TRequest> Register<TRequest, TResponse>(RequestHandlerDelegate<TRequest, TResponse> requestHandlerDelegate)
			where TRequest : IRequest<TResponse>
			where TResponse : class, IResponse
		{
			IRequestPipelineBuilder<TRequest> requestPipelineBuilder = new BasicRequestPipelineBuilder<TRequest>();

			lock (_lock)
			{
				_requestHandlerDictionary[typeof(TRequest)] = (request) =>
				{
					requestPipelineBuilder.Build().ExecutePipelineQueue((TRequest)request);

					return requestHandlerDelegate((TRequest)request);
				};
			}

			return requestPipelineBuilder;
		}

		protected RequestExecutorBase()
		{
			Register();
		}

		public virtual TResponse Execute<TResponse>(IRequest<TResponse> request)
			where TResponse : class, IResponse
		{
			RequestHandlerDelegate<IRequest<IResponse>, IResponse> handlerDelegate;

			lock (_lock)
			{
				handlerDelegate = _requestHandlerDictionary[request.GetType()];
			}

			return (TResponse)handlerDelegate(request);
		}

		public virtual Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request)
			where TResponse : class, IResponse
		{
			RequestHandlerDelegate<IRequest<IResponse>, IResponse> handlerDelegate;

			lock (_lock)
			{
				handlerDelegate = _requestHandlerDictionary[request.GetType()];
			}

			return Task.Factory.StartNew<TResponse>((r) => (TResponse)handlerDelegate((IRequest<TResponse>)r), request);
		}

		public abstract void Register();
	}
}
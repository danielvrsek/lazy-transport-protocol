using LazyTransportProtocol.Core.Application.Pipeline;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Pipeline;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application
{
	public abstract class RequestExecutorBase : IRequestExecutor
	{
		protected Dictionary<Type, RequestHandlerDelegate<IRequest<IResponse>, IResponse>> _requestHandlerDictionary = new Dictionary<Type, RequestHandlerDelegate<IRequest<IResponse>, IResponse>>();

		public virtual IPipelineBuilder<TRequest> Register<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> requestHandler)
			where TRequest : IRequest<TResponse>
			where TResponse : class, IResponse
		{
			BasicRequestPipelineBuilder<TRequest, TResponse> requestPipelineBuilder = new BasicRequestPipelineBuilder<TRequest, TResponse>();

			// Register delegate - don't execute
			_requestHandlerDictionary[typeof(TRequest)] = (request) =>
			{
				// Execute pipeline
				request = requestPipelineBuilder.Build().ExecutePipelineQueue((TRequest)request);

				return requestHandler.GetResponse((TRequest)request);
			};

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

			handlerDelegate = _requestHandlerDictionary[request.GetType()];

			return (TResponse)handlerDelegate(request);
		}

		public virtual Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request)
			where TResponse : class, IResponse
		{
			return Task.Factory.StartNew((r) => Execute((IRequest<TResponse>)r), request);
		}

		public abstract void Register();

		bool IRequestExecutor.CanExecute<TResponse>(IRequest<TResponse> request)
		{
			return _requestHandlerDictionary.ContainsKey(request.GetType());
		}
	}
}
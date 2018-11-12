using LazyTransportProtocol.Core.Application.Pipeline;
using LazyTransportProtocol.Core.Application.Protocol.Attributes;
using LazyTransportProtocol.Core.Application.Protocol.Handlers;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Pipeline;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Protocol
{
	public class ProtocolRequestExecutor : IRequestExecutor
	{
		public static object _lock = new object();

		protected static Dictionary<string, RequestHandlerDelegate<IRequest<IResponse>, IResponse>> _requestHandlerDictionary = new Dictionary<string, RequestHandlerDelegate<IRequest<IResponse>, IResponse>>();

		public ProtocolRequestExecutor()
		{
			Register();
		}

		public static IPipelineBuilder<TRequest> Register<TRequest, TResponse>(string identifier, IRequestHandler<TRequest, TResponse> requestHandler)
			where TRequest : IRequest<TResponse>
			where TResponse : class, IResponse
		{
			IPipelineBuilder<TRequest> requestPipelineBuilder = new BasicRequestPipelineBuilder<TRequest>();

			lock (_lock)
			{
				_requestHandlerDictionary[identifier] = (request) =>
				{
					requestPipelineBuilder.Build().ExecutePipelineQueue((TRequest)request);

					return requestHandler.GetResponse((TRequest)request);
				};
			}

			return requestPipelineBuilder;
		}

		public static IPipelineBuilder<TRequest> Register<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> requestHandler)
			where TRequest : IRequest<TResponse>
			where TResponse : class, IResponse
		{
			string identifier = GetIdentifier<TRequest>();

			return Register(identifier, requestHandler);
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

		public virtual TResponse Execute<TResponse>(string identifier, IRequest<TResponse> request)
			where TResponse : class, IResponse
		{
			RequestHandlerDelegate<IRequest<IResponse>, IResponse> handlerDelegate;

			lock (_lock)
			{
				handlerDelegate = _requestHandlerDictionary[identifier];
			}

			return (TResponse)handlerDelegate(request);
		}

		public virtual Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request)
			where TResponse : class, IResponse
		{
			return Task.Factory.StartNew<TResponse>((r) => (TResponse)Execute((IRequest<TResponse>)r), request);
		}

		public bool CanExecute<TResponse>(IRequest<TResponse> request)
			where TResponse : class, IResponse
		{
			throw new NotImplementedException();
		}

		private static string GetIdentifier<T>()
		{
			var attribute = (IdentifierAttribute)typeof(T).GetCustomAttributes(typeof(IdentifierAttribute), true).Single();
			return attribute.Identifier;
		}

		public void Register()
		{
			Register(new AuthorizationRequestHandler());
			Register(new ListDirectoryRequestHandler());
			Register(new CreateUserRequestHandler());
		}
	}
}
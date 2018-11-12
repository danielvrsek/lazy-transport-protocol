using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Domain.Abstractions
{
	public delegate TResponse RequestHandlerDelegate<TRequest, TResponse>(TRequest request)
		where TRequest : IRequest<TResponse>
		where TResponse : class, IResponse;

	public interface IRequestExecutor
	{
		bool CanExecute<TResponse>(IRequest<TResponse> request)
			where TResponse : class, IResponse;

		TResponse Execute<TResponse>(IRequest<TResponse> request)
			where TResponse : class, IResponse;

		Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request)
			where TResponse : class, IResponse;
	}
}
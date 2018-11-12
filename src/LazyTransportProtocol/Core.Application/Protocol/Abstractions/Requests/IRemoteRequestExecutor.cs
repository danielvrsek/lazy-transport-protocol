using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests
{
	public interface IRemoteRequestExecutor
	{
		TResponse Execute<TResponse>(IProtocolRequest<TResponse> request)
			where TResponse : class, IProtocolResponse, new();

		Task<TResponse> ExecuteAsync<TResponse>(IProtocolRequest<TResponse> request)
			where TResponse : class, IProtocolResponse, new();
	}
}
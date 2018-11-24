using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Model;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests
{
	/// <summary>
	/// Interface to provide abstraction above transport layer (e.g. Socket transport layer)
	/// </summary>
	public interface IRemoteRequestExecutor
	{
		/// <summary>
		/// Method for connection to the remote host
		/// </summary>
		/// <param name="connectionParameters">Parameters of the connection</param>
		void Connect(IRemoteConnectionParameters connectionParameters);

		/// <summary>
		/// Remotely execute request
		/// </summary>
		/// <typeparam name="TResponse">Type of the response</typeparam>
		/// <param name="request">Request to execute on remote host</param>
		/// <returns>Response of the remote host</returns>
		TResponse Execute<TResponse>(IProtocolRequest<TResponse> request)
			where TResponse : class, IProtocolResponse, new();

		/// <summary>
		/// Remotely execute request
		/// </summary>
		/// <typeparam name="TResponse">Type of the response</typeparam>
		/// <param name="request">Request to execute on remote host</param>
		/// <returns>Response of the remote host</returns>
		Task<TResponse> ExecuteAsync<TResponse>(IProtocolRequest<TResponse> request)
			where TResponse : class, IProtocolResponse, new();

		/// <summary>
		/// Method for ending the connection with the remote host
		/// </summary>
		void Disconnect();
	}
}
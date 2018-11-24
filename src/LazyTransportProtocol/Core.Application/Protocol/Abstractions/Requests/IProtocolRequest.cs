using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Infrastructure;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using System.Collections.Generic;

namespace LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests
{
	/// <summary>
	/// Abstraction for protocol requests.
	/// </summary>
	/// <typeparam name="TResponse">Type of the response</typeparam>
	public interface IProtocolRequest<out TResponse> : IRequest<TResponse>
		where TResponse : IProtocolResponse
	{
		IAuthenticationContext AuthenticationContext { get; set; }

		/// <summary>
		/// Method to obtain identifier of the protocol request
		/// </summary>
		/// <param name="protocolVersion">Protocol version</param>
		/// <returns>Identifier of the protocol request</returns>
		string GetIdentifier(ProtocolVersion protocolVersion);
	}
}
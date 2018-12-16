using LazyTransportProtocol.Core.Application.Protocol.Responses.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;

namespace LazyTransportProtocol.Core.Application.Protocol.Requests.Abstractions
{
	/// <summary>
	/// Abstraction for protocol requests.
	/// </summary>
	/// <typeparam name="TResponse">Type of the response</typeparam>
	public interface IProtocolRequest<out TResponse> : IRequest<TResponse>
		where TResponse : IProtocolResponse
	{
		/// <summary>
		/// Method to obtain identifier of the protocol request
		/// </summary>
		string GetIdentifier();
	}
}
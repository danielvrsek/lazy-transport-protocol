using LazyTransportProtocol.Core.Domain.Abstractions.Responses;

namespace LazyTransportProtocol.Core.Application.Protocol.Responses.Abstractions
{
	/// <summary>
	/// Interface for all protocol responses to implement
	/// </summary>
	public interface IProtocolResponse : IResponse
	{
		/// <summary>
		/// Method to obtain identifier of the protocol response
		/// </summary>
		/// <param name="protocolVersion">Protocol version</param>
		/// <returns>Identifier of the protocol response</returns>
		string GetIdentifier();
	}
}
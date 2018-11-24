using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses
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
		string GetIdentifier(ProtocolVersion protocolVersion);
	}
}
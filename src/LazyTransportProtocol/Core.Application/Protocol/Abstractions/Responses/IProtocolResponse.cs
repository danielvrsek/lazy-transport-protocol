using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses
{
	public interface IProtocolResponse : IResponse
	{
		string Serialize(ProtocolVersion protocolVersion);

		void Deserialize(string data, ProtocolVersion protocolVersion);
	}
}
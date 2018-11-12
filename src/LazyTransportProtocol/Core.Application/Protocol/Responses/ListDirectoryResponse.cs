using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Responses
{
	public class ListDirectoryResponse : IProtocolResponse
	{
		public string Serialize(ProtocolVersion protocolVersion)
		{
			throw new NotImplementedException();
		}
	}
}
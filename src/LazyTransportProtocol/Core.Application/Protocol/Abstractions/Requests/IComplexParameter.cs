using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests
{
	public interface IComplexParameter
	{
		string Serialize(ProtocolVersion protocolVersion);
	}
}
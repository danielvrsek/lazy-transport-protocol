using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Model
{
	public class RemoteDirectory : IComplexParameter
	{
		public string Name { get; set; }

		public string Serialize(ProtocolVersion protocolVersion)
		{
			return $"name={Name}";
		}
	}
}
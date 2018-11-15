using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Configuration;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Configuration
{
	public class ProtocolConfiguration : IProtocolConfiguration
	{
		public ProtocolVersion ProtocolVersion => ProtocolVersion.V1_0;

		public char Separator => ';';
	}
}
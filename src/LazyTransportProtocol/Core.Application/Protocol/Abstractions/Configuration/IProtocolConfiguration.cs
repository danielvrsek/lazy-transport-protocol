using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Abstractions.Configuration
{
	public interface IProtocolConfiguration
	{
		ProtocolVersion ProtocolVersion { get; }

		char Separator { get; }
	}
}
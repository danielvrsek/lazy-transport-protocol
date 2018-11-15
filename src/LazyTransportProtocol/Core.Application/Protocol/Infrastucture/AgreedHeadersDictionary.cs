using LazyTransportProtocol.Core.Application.Protocol.Metadata;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Infrastucture
{
	public class AgreedHeadersDictionary : Dictionary<string, string>
	{
		public string Separator
		{
			get
			{
				return this[HandshakeValuesMetadata.ControlSeparator];
			}
		}

		public ProtocolVersion ProtocolVersion
		{
			get
			{
				return new ProtocolVersion(this[HandshakeValuesMetadata.ProtocolVersion]);
			}
		}
	}
}
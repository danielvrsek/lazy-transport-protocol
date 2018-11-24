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
		public AgreedHeadersDictionary()
		{
		}

		public AgreedHeadersDictionary(string separator, int maxRequestLength, ProtocolVersion protocolVersion)
		{
			this[HandshakeHeadersMetadata.ControlSeparator] = separator;
			this[HandshakeHeadersMetadata.MaxRequestLength] = maxRequestLength.ToString();
			this[HandshakeHeadersMetadata.ProtocolVersion] = protocolVersion.ToString();
		}

		public string Separator
		{
			get
			{
				return this[HandshakeHeadersMetadata.ControlSeparator];
			}
		}

		public int MaxRequestLength
		{
			get
			{
				return Int32.Parse(this[HandshakeHeadersMetadata.MaxRequestLength]);
			}
		}

		public ProtocolVersion ProtocolVersion
		{
			get
			{
				return new ProtocolVersion(this[HandshakeHeadersMetadata.ProtocolVersion]);
			}
		}
	}
}
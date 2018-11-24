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

		public AgreedHeadersDictionary(string separator, int bufferLength, ProtocolVersion protocolVersion)
		{
			this[HandshakeHeadersMetadata.ControlSeparator] = separator;
			this[HandshakeHeadersMetadata.BufferSize] = bufferLength.ToString();
			this[HandshakeHeadersMetadata.ProtocolVersion] = protocolVersion.ToString();
		}

		public string Separator
		{
			get
			{
				return this[HandshakeHeadersMetadata.ControlSeparator];
			}
		}

		public int BufferSize
		{
			get
			{
				return Int32.Parse(this[HandshakeHeadersMetadata.BufferSize]);
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
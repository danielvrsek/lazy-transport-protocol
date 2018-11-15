using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Model
{
	public class RemoteFile : IComplexParameter
	{
		public string Filename { get; set; }

		public int Size { get; set; }

		public string Serialize(ProtocolVersion protocolVersion)
		{
			return $"filename={Filename},size={Size}";
		}
	}
}
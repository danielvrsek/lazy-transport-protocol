using LazyTransportProtocol.Core.Domain.Abstractions.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class ProtocolDecoder : IDecoder
	{
		public string Decode(byte[] data)
		{
			return UTF8Encoding.UTF8.GetString(data);
		}
	}
}
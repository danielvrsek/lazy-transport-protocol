using LazyTransportProtocol.Core.Domain.Abstractions.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class ProtocolEncoder : IEncoder
	{
		public byte[] Encode(string data)
		{
			return UTF8Encoding.UTF8.GetBytes(data);
		}
	}
}
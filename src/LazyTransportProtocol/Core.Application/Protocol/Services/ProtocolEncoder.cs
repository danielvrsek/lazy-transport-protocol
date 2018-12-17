using LazyTransportProtocol.Core.Domain.Abstractions.Common;
using System;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class ProtocolEncoder : IEncoder, IDecoder
	{
		public byte[] Encode(string data)
		{
			return Encoding.UTF8.GetBytes(data);
		}

		public string Decode(byte[] data)
		{
			return Decode(data, 0, data.Length);
		}

		public string Decode(byte[] data, int index, int count)
		{
			return Decode(new ArraySegment<byte>(data, index, count));
		}

		public string Decode(ArraySegment<byte> data)
		{
			return Encoding.UTF8.GetString(data.Array, data.Offset, data.Count);
		}
	}
}
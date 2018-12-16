using LazyTransportProtocol.Core.Domain.Abstractions.Common;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class ProtocolEncoder : IEncoder, IDecoder
	{
		public byte[] Encode(string data)
		{
			return UTF8Encoding.UTF8.GetBytes(data);
		}

		public string Decode(byte[] data)
		{
			return Decode(data, 0, data.Length);
		}

		public string Decode(byte[] data, int index, int count)
		{
			return UTF8Encoding.UTF8.GetString(data, index, count);
		}
	}
}
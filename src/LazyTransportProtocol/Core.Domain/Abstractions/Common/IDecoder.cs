using System;

namespace LazyTransportProtocol.Core.Domain.Abstractions.Common
{
	public interface IDecoder
	{
		string Decode(byte[] data);

		string Decode(byte[] data, int index, int count);

		string Decode(ArraySegment<byte> data);
	}
}
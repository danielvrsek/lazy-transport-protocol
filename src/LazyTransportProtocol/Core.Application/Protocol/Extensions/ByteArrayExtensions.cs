using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Extensions
{
	public static class ByteArrayExtensions
	{
		public static bool EndsWith(this ArraySegment<byte> segment, byte[] data)
		{
			if (segment.Count < data.Length)
			{
				return false;
			}

			int segmentIndex = segment.Count - data.Length;

			for (int i = 0; i < data.Length; i++, segmentIndex++)
			{
				if (segment[segmentIndex] != data[i])
				{
					return false;
				}
			}

			return true;
		}

		public static bool EndsWith(this byte[] segment, byte[] data)
		{
			return EndsWith(new ArraySegment<byte>(segment), data);
		}
	}
}

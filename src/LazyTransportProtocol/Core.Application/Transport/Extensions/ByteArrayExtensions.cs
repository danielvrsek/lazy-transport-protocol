using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Transport.Extensions
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
				if (segment.Array[segment.Offset + segmentIndex] != data[i])
				{
					return false;
				}
			}

			return true;
		}

		public static bool EndsWith(this byte[] array, byte[] data)
		{
			return EndsWith(new ArraySegment<byte>(array), data);
		}

		public static byte[] Append(this byte[] array, byte[] val)
		{
			int arrLength = array.Length;
			byte[] data = new byte[arrLength + val.Length];

			for (int i = 0; i < arrLength; i++)
			{
				data[i] = array[i];
			}

			for (int i = 0; i < val.Length; i++)
			{
				data[arrLength + i] = val[i];
			}

			return data;
		}
	}
}
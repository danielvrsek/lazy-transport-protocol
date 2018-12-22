using System;
using System.Collections.Generic;

namespace LazyTransportProtocol.Core.Application.Transport.Services
{
	internal static class TransportSerializer
	{
		public static IList<ArraySegment<byte>> Serialize(IList<ArraySegment<byte>> data)
		{
			int transportDataLength = 0;
			foreach (var segment in data)
			{
				transportDataLength += segment.Count;
			}
			// Possible optimization would be to add datalength buffer and set its value after
			ArraySegment<byte> dataLength = new ArraySegment<byte>(BitConverter.GetBytes(transportDataLength));
			List<ArraySegment<byte>> transportData = new List<ArraySegment<byte>>();
			transportData.Add(dataLength);
			transportData.AddRange(data);

			return transportData;
		}
	}
}
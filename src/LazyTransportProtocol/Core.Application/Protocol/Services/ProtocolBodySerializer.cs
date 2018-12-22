using MessagePack;
using MessagePack.Resolvers;
using System;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class ProtocolBodySerializer
	{
		public static byte[] Serialize<T>(T request)
		{
			if (request == null)
			{
				throw new ArgumentException("Argument cannot be null.", nameof(request));
			}

			return MessagePackSerializer.Serialize<object>(request, ContractlessStandardResolver.Instance);
		}

		public static T Deserialize<T>(ArraySegment<byte> requestBody)
		{
			if (requestBody == null)
			{
				throw new ArgumentException("Argument cannot be null.", nameof(requestBody));
			}

			return MessagePackSerializer.Deserialize<T>(requestBody, ContractlessStandardResolver.Instance);
		}
	}
}
using LazyTransportProtocol.Core.Domain.Abstractions.Common;
using MessagePack;
using MessagePack.Resolvers;
using System;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class ProtocolBodySerializer
	{
		public static byte[] Serialize<T>(T request)
		{
			return SerializeInternal(request);
		}

		private static byte[] SerializeInternal<T>(T obj)
		{
			if (obj == null)
			{
				throw new ArgumentException("Argument cannot be null.", nameof(obj));
			}

			IEncoder protocolEncoder = new ProtocolEncoder();

			return MessagePackSerializer.Serialize<object>(obj, ContractlessStandardResolver.Instance);
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
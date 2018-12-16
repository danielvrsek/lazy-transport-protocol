using LazyTransportProtocol.Core.Application.Protocol.Requests.Abstractions;
using LazyTransportProtocol.Core.Application.Protocol.Responses.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Common;
using Newtonsoft.Json;
using System;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class ProtocolBodySerializer
	{
		public static string Serialize<TResponse>(IProtocolRequest<TResponse> request)
			where TResponse : class, IProtocolResponse, new()
		{
			return SerializeInternal(request);
		}

		public static string Serialize(IProtocolResponse response)
		{
			return SerializeInternal(response);
		}

		private static string SerializeInternal<T>(T obj)
		{
			if (obj == null)
			{
				throw new ArgumentException("Argument cannot be null.", nameof(obj));
			}

			IEncoder protocolEncoder = new ProtocolEncoder();

			string serializedRequest = JsonConvert.SerializeObject(obj);

			return Convert.ToBase64String(protocolEncoder.Encode(serializedRequest));
		}

		public static T Deserialize<T>(string requestBody)
		{
			if (requestBody == null)
			{
				throw new ArgumentException("Argument cannot be null.", nameof(requestBody));
			}

			IDecoder decoder = new ProtocolEncoder();
			string decoded = decoder.Decode(Convert.FromBase64String(requestBody));

			return JsonConvert.DeserializeObject<T>(decoded);
		}
	}
}
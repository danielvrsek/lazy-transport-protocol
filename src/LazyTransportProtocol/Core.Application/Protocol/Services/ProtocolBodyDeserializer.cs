using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Domain.Abstractions.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public static class ProtocolBodyDeserializer
	{
		public static T Deserialize<T>(string requestBody)
		{
			if (requestBody == null)
			{
				throw new ArgumentException("Argument cannot be null.", nameof(requestBody));
			}

			IDecoder decoder = new ProtocolDecoder();
			string decoded = decoder.Decode(Convert.FromBase64String(requestBody));

			return JsonConvert.DeserializeObject<T>(decoded);
		}
	}
}
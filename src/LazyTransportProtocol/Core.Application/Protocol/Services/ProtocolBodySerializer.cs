using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Domain.Abstractions.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class ProtocolBodySerializer
	{
		public string Serialize<TResponse>(IProtocolRequest<TResponse> request, ProtocolVersion protocolVersion)
			where TResponse : class, IProtocolResponse, new()
		{
			return SerializeInternal(request);
		}

		public string Serialize(IProtocolResponse response, ProtocolVersion protocolVersion)
		{
			return SerializeInternal(response);
		}

		private string SerializeInternal<T>(T obj)
		{
			if (obj == null)
			{
				throw new ArgumentException("Argument cannot be null.", nameof(obj));
			}

			IEncoder protocolEncoder = new ProtocolEncoder();

			string serializedRequest = JsonConvert.SerializeObject(obj);

			return Convert.ToBase64String(protocolEncoder.Encode(serializedRequest));
		}
	}
}
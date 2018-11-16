using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.Metadata;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Domain.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace LazyTransportProtocol.Core.Application.Protocol.Extensions
{
	public static class RequestHelper
	{
		public static T Deserialize<T>(string requestString, ProtocolVersion protocolVersion)
		{
			if (requestString == null)
			{
				throw new ArgumentException("Argument cannot be null.", nameof(requestString));
			}

			return JsonConvert.DeserializeObject<T>(requestString);
		}

		public static string Serialize<TResponse>(this IProtocolRequest<TResponse> request, AgreedHeadersDictionary headers, ProtocolVersion protocolVersion)
			where TResponse : class, IProtocolResponse, new()
		{
			if (request == null)
			{
				throw new ArgumentException("Argument cannot be null.", nameof(request));
			}

			if (headers == null)
			{
				throw new ArgumentException("Argument cannot be null.", nameof(headers));
			}

			return JsonConvert.SerializeObject(request);
		}
	}
}
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.Metadata;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Domain.Exceptions;
using LazyTransportProtocol.Core.Domain.Exceptions.Request;
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
		/// <summary>
		/// Deserializes request
		/// </summary>
		/// <typeparam name="T">Type of the request</typeparam>
		/// <param name="requestBody">Decoded body of the request</param>
		/// <param name="protocolVersion">Protocol version</param>
		/// <exception cref="RequestBodyDeserializationException"/>
		/// <returns>Deserialized request</returns>
		public static T Deserialize<T>(string requestBody, ProtocolVersion protocolVersion)
		{
			if (requestBody == null)
			{
				throw new ArgumentException("Argument cannot be null.", nameof(requestBody));
			}

			T request;
			try
			{
				request = JsonConvert.DeserializeObject<T>(requestBody);
			}
			catch
			{
				throw new RequestBodyDeserializationException();
			}

			return request;
		}

		/// <summary>
		/// Serializes request to JSON string
		/// </summary>
		/// <param name="request">The request to serialize</param>
		/// <param name="headers">Agreed headers from handshake</param>
		/// <param name="protocolVersion">Protocol version</param>
		/// <returns>Serialized JSON string</returns>
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

			// We dont want to throw custom exceptions, because request serialization is handled on the client
			return JsonConvert.SerializeObject(request); 
		}
	}
}
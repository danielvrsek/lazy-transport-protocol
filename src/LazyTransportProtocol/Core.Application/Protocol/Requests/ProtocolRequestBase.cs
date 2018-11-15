using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Requests
{
	public abstract class ProtocolRequestBase<TResponse> : IProtocolRequest<TResponse>
		where TResponse : class, IProtocolResponse, new()
	{
		public string Serialize(AgreedHeadersDictionary headers, ProtocolVersion protocolVersion)
		{
			if (headers == null)
			{
				throw new ArgumentException("Argument cannot be null.", nameof(headers));
			}

			return GetIdentifier(protocolVersion) + headers.Separator + SerializeInternal(headers, protocolVersion);
		}

		public void Deserialize(MediumDeserializedRequestObject requestObject, ProtocolVersion protocolVersion)
		{
			if (requestObject == null)
			{
				throw new ArgumentException("Argument cannot be null.", nameof(requestObject));
			}

			if (requestObject.ControlCommand != GetIdentifier(protocolVersion))
			{
				throw new RequestMismatchException();
			}

			DeserializeInternal(requestObject, protocolVersion);
		}

		public abstract string GetIdentifier(ProtocolVersion protocolVersion);

		protected abstract string SerializeInternal(AgreedHeadersDictionary headers, ProtocolVersion protocolVersion);

		protected abstract void DeserializeInternal(MediumDeserializedRequestObject requestObject, ProtocolVersion protocolVersion);
	}
}
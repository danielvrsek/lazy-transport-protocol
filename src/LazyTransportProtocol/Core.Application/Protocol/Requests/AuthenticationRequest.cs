using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.Metadata;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Exceptions;
using LazyTransportProtocol.Core.Transport.Infrastructure;

namespace LazyTransportProtocol.Core.Application.Protocol.Requests
{
	public class AuthenticationRequest : ProtocolRequestBase<AcknowledgementResponse>
	{
		public const string Identifier = "AUTHENTICATE";

		public string Username { get; set; }

		public string Password { get; set; }

		public override string GetIdentifier(ProtocolVersion protocolVersion)
		{
			return Identifier;
		}

		protected override string SerializeInternal(AgreedHeadersDictionary headers, ProtocolVersion protocolVersion)
		{
			string separator = headers[HandshakeValuesMetadata.ControlSeparator];

			return GetIdentifier(protocolVersion) + separator + $"username={Username}&password={Password}";
		}

		protected override void DeserializeInternal(MediumDeserializedRequestObject requestObject, ProtocolVersion protocolVersion)
		{
			Username = requestObject.Parameters.Get("username");
			Password = requestObject.Parameters.Get("password");
		}
	}
}
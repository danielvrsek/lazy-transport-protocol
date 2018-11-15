﻿using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.Metadata;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;

namespace LazyTransportProtocol.Core.Application.Protocol.Requests
{
	public class CreateUserRequest : ProtocolRequestBase<AcknowledgementResponse>
	{
		public const string Identifier = "CREATEUSER";

		public string Username { get; set; }

		public string Password { get; set; }

		public override string GetIdentifier(ProtocolVersion protocolVersion)
		{
			return Identifier;
		}

		protected override string SerializeInternal(AgreedHeadersDictionary headers, ProtocolVersion protocolVersion)
		{
			string separator = headers.Separator;

			return GetIdentifier(protocolVersion) + separator + $"username={Username}&password={Password}";
		}

		protected override void DeserializeInternal(MediumDeserializedRequestObject requestObject, ProtocolVersion protocolVersion)
		{
			Username = requestObject.Parameters.Get("username");
			Password = requestObject.Parameters.Get("password");
		}
	}
}
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.Metadata;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using System.Collections.Generic;

namespace LazyTransportProtocol.Core.Application.Protocol.Requests
{
	public class ListDirectoryClientRequest : ProtocolRequestBase<ListDirectoryResponse>
	{
		public const string Identifier = "HANDSHAKE";

		public string Path { get; set; }

		public override string GetIdentifier(ProtocolVersion protocolVersion)
		{
			return Identifier;
		}

		protected override string SerializeInternal(AgreedHeadersDictionary headers, ProtocolVersion protocolVersion)
		{
			string separator = headers[HandshakeValuesMetadata.ControlSeparator];

			return GetIdentifier(protocolVersion) + separator + $"path={Path}";
		}

		protected override void DeserializeInternal(MediumDeserializedRequestObject requestObject, ProtocolVersion protocolVersion)
		{
			Path = requestObject.Parameters.Get("path");
		}
	}
}
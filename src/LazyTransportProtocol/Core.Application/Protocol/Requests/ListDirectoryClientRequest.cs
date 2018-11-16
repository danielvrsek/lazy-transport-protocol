using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.Metadata;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using System.Collections.Generic;

namespace LazyTransportProtocol.Core.Application.Protocol.Requests
{
	public class ListDirectoryClientRequest : IProtocolRequest<ListDirectoryResponse>
	{
		public const string Identifier = "HANDSHAKE";

		public string Path { get; set; }

		public string GetIdentifier(ProtocolVersion protocolVersion)
		{
			return Identifier;
		}
	}
}
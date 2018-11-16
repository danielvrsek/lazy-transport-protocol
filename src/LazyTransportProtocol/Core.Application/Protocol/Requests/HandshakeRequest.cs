using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.Metadata;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Exceptions;
using LazyTransportProtocol.Core.Transport.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Requests
{
	public class HandshakeRequest : IProtocolRequest<AcknowledgementResponse>
	{
		public const string Identifier = "HANDSHAKE";

		public ProtocolVersion ProtocolVersion { get; set; }

		public string Separator { get; set; }

		public string GetIdentifier(ProtocolVersion protocolVersion)
		{
			return Identifier;
		}
	}
}
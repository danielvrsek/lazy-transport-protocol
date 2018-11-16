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
	public class AuthenticationRequest : IProtocolRequest<AcknowledgementResponse>
	{
		public const string Identifier = "AUTHENTICATE";

		public string Username { get; set; }

		public string Password { get; set; }

		public string GetIdentifier(ProtocolVersion protocolVersion)
		{
			return Identifier;
		}
	}
}
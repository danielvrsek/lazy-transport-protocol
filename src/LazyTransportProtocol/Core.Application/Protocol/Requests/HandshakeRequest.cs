using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Transport.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Requests
{
	public class HandshakeRequest : IProtocolRequest<AcknowledgementResponse>
	{
		public IConnection Connection { get; set; }

		public ProtocolVersion ProtocolVersion { get; set; }

		public char Separator { get; set; }

		public string Serialize(ProtocolVersion protocolVersion)
		{
			return $"HELLO {ProtocolVersion} {Separator}";
		}
	}
}
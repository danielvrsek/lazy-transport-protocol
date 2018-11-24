using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Transport;
using LazyTransportProtocol.Core.Application.Transport.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Handlers
{
	public class HandshakeRequestHandler : IProtocolRequestHandler<HandshakeRequest, AcknowledgementResponse>
	{
		public AcknowledgementResponse GetResponse(HandshakeRequest request)
		{
			Console.WriteLine($"Handshake. Data: {request.ProtocolVersion}, {request.Separator}");

			return new AcknowledgementResponse
			{
				Code = 200
			};
		}
	}
}
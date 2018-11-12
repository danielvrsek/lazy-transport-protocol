using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Configuration;
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
		private IRequestExecutor transportExecutor = new TransportRequestExecutor();
		private IRequestExecutor protocolExecutor = new RemoteProtocolRequestExecutor();
		private IProtocolConfiguration protocolConfiguration = new ProtocolConfiguration();
		private IEncoder protocolEncoder = new ProtocolEncoder();
		private IDecoder protocolDecoder = new ProtocolDecoder();

		public AcknowledgementResponse GetResponse(HandshakeRequest request)
		{
			byte[] bytes = protocolEncoder.Encode(request.Serialize(protocolConfiguration.ProtocolVersion));
			string response = protocolDecoder.Decode(request.Connection.Send(bytes));

			string[] flags = response.Split(' ');

			return new AcknowledgementResponse
			{
				IsSuccessful = flags[0] == "OK"
			};
		}
	}
}
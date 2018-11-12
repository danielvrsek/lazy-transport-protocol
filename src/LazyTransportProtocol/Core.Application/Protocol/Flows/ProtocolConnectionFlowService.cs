using LazyTransportProtocol.Core.Application.Flow;
using LazyTransportProtocol.Core.Application.Infrastructure;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Configuration;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Application.Transport;
using LazyTransportProtocol.Core.Application.Transport.DataModel;
using LazyTransportProtocol.Core.Application.Transport.Requests;
using LazyTransportProtocol.Core.Application.Transport.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Common;
using LazyTransportProtocol.Core.Transport.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Flow
{
	public class ProtocolConnectionFlowService
	{
		private TransportConnectionFlowService transportFlow = new TransportConnectionFlowService();
		private IRequestExecutor transportExecutor = new TransportRequestExecutor();
		private IRequestExecutor protocolExecutor = new RemoteProtocolRequestExecutor();
		private IProtocolConfiguration protocolConfiguration = new ProtocolConfiguration();
		private IEncoder protocolEncoder = new ProtocolEncoder();
		private IDecoder protocolDecoder = new ProtocolDecoder();

		private IConnection connection = null;

		public void Authenticate(string username, string password)
		{
		}

		public void StartConnection()
		{
			ITransport transport = new TransportLayer();

			connection = transport.Connect("127.0.0.1", 1234);
		}

		public void BeginHandshake()
		{
			var response = protocolExecutor.Execute(new HandshakeRequest
			{
				Connection = connection,
				ProtocolVersion = ProtocolVersion.V1_0,
				Separator = ';'
			});

			Console.WriteLine(response.IsSuccessful ? "Successfully connected." : "Connection failed.");
		}

		public void EndConnection()
		{
			connection.Disconnect();
		}
	}
}
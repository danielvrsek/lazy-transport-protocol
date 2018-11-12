using LazyTransportProtocol.Core.Application.Transport;
using LazyTransportProtocol.Core.Application.Transport.DataModel;
using LazyTransportProtocol.Core.Application.Transport.Requests;
using LazyTransportProtocol.Core.Application.Transport.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Flow
{
	public class TransportConnectionFlowService
	{
		public BeginConnectDto BeginConnect(string internetAddress, int port)
		{
			var request = new ConnectToServerRequest
			{
				IpAdress = internetAddress,
				Port = port
			};

			ConnectToServerResponse response = new TransportRequestExecutor().Execute(request);

			return new BeginConnectDto
			{
				ConnectionContext = response.Connection
			};
		}
	}
}
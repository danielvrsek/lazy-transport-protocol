using LazyTransportProtocol.Core.Application.Transport;
using LazyTransportProtocol.Core.Application.Transport.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Flow
{
	public class ConnectionFlowService
	{
		public bool BeginConnect(string internetAddress, int port)
		{
			var request = new ConnectToServerRequest
			{
				IpAdress = internetAddress,
				Port = port
			};

			new TransportRequestExecutor().Execute(request);

			return true;
		}
	}
}
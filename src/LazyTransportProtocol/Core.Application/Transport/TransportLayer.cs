using System;
using System.Collections.Generic;
using System.Text;
using LazyTransportProtocol.Core.Application.Transport.Requests;
using LazyTransportProtocol.Core.Application.Transport.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions;

namespace LazyTransportProtocol.Core.Application.Transport
{
	public class TransportLayer : ITransport
	{
		private TransportRequestExecutor _transportExecutor = new TransportRequestExecutor();

		public IConnection Connect(string ipAdress, int port)
		{
			ConnectToServerResponse response = _transportExecutor.Execute(new ConnectToServerRequest
			{
				IpAdress = ipAdress,
				Port = port
			});

			return response.Connection;
		}
	}
}
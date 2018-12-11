using LazyTransportProtocol.Core.Application.Transport.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Transport.Responses;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Transport.Requests
{
	public class ConnectToServerRequest : ITransportRequest<ConnectToServerResponse>
	{
		public IPAddress IPAdress { get; set; }

		public int Port { get; set; }
	}
}
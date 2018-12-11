using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Model
{
	public class SocketConnectionParameters : IRemoteConnectionParameters
	{
		public IPAddress IPAddress { get; set; }

		public int Port { get; set; }
	}
}
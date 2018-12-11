using LazyTransportProtocol.Core.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Transport
{
	public interface ITransport
	{
		IServerConnection Connect(IPAddress ipAdress, int port);
	}
}
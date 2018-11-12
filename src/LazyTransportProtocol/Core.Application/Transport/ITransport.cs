using LazyTransportProtocol.Core.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Transport
{
	public interface ITransport
	{
		IConnection Connect(string ipAdress, int port);
	}
}
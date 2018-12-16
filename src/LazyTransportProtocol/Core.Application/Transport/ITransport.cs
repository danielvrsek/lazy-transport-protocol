using LazyTransportProtocol.Core.Application.Transport.Model;
using LazyTransportProtocol.Core.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Transport
{
	public delegate void ClientConnected(IClientConnection connection);

	public delegate void DataReceived(IClientConnection connection, byte[] data);

	public delegate void ErrorOccured(ErrorContext ctx);

	public interface ITransport
	{
		void Listen(IPAddress ipAddress, int port, ClientConnected clientConnected, DataReceived dataReceived, ErrorOccured errorOccured);

		IServerConnection Connect(IPAddress ipAdress, int port);
	}
}
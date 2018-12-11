using LazyTransportProtocol.Core.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Transport.Infrastructure
{
	public class SocketClientConnection : IClientConnection
	{
		private readonly Action<byte[]> _send;
		private readonly Action _disconnect;

		public SocketClientConnection(Action<byte[]> send, Action disconnect)
		{
			_send = send;
			_disconnect = disconnect;
		}

		public void Send(byte[] data)
		{
			_send(data);
		}

		public void Disconnect()
		{
			_disconnect();
		}
	}
}
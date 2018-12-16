using LazyTransportProtocol.Core.Domain.Abstractions;
using System;

namespace LazyTransportProtocol.Core.Application.Transport.Infrastructure
{
	internal class SocketClientConnection : IClientConnection
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
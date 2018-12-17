using LazyTransportProtocol.Core.Domain.Abstractions;
using System;

namespace LazyTransportProtocol.Core.Application.Transport.Infrastructure
{
	internal class SocketClientConnection : IClientConnection
	{
		private readonly Action<ArraySegment<byte>> _send;
		private readonly Action _disconnect;

		public SocketClientConnection(Action<ArraySegment<byte>> send, Action disconnect)
		{
			_send = send;
			_disconnect = disconnect;
		}

		public void Send(ArraySegment<byte> data)
		{
			_send(data);
		}

		public void Disconnect()
		{
			_disconnect();
		}
	}
}
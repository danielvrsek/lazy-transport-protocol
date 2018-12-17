using LazyTransportProtocol.Core.Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace LazyTransportProtocol.Core.Application.Transport.Infrastructure
{
	internal class SocketClientConnection : IClientConnection
	{
		private readonly Action<IList<ArraySegment<byte>>> _send;
		private readonly Action _disconnect;

		public SocketClientConnection(Action<IList<ArraySegment<byte>>> send, Action disconnect)
		{
			_send = send;
			_disconnect = disconnect;
		}

		public void Send(IList<ArraySegment<byte>> data)
		{
			_send(data);
		}

		public void Disconnect()
		{
			_disconnect();
		}
	}
}
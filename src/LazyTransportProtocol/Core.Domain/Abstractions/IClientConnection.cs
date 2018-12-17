using System;

namespace LazyTransportProtocol.Core.Domain.Abstractions
{
	public interface IClientConnection
	{
		void Send(ArraySegment<byte> data);

		void Disconnect();
	}
}
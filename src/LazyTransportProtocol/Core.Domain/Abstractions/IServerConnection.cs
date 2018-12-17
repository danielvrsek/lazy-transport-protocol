using System;

namespace LazyTransportProtocol.Core.Domain.Abstractions
{
	public interface IServerConnection
	{
		byte[] Send(ArraySegment<byte> data);

		void SendAsync(ArraySegment<byte> data, Action<byte[]> responseCallback);

		void Disconnect();

		bool IsAlive();
	}
}
using System;
using System.Net.Sockets;

namespace LazyTransportProtocol.Core.Domain.Abstractions
{
	public interface IServerConnection
	{
		byte[] Send(byte[] data);

		void SendAsync(byte[] data, Action<byte[]> responseCallback);

		void Disconnect();

		bool IsAlive();
	}
}
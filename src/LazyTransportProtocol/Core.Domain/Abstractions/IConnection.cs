using System;
using System.Net.Sockets;

namespace LazyTransportProtocol.Core.Domain.Abstractions
{
	public interface IConnection
	{
		IConnectionState State { get; }

		byte[] Send(byte[] data);

		void Send(byte[] data, Action<byte[]> responseCallback);

		void Disconnect();

		bool IsAlive();
	}
}
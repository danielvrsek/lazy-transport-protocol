using System;
using System.Net.Sockets;

namespace LazyTransportProtocol.Core.Domain.Abstractions
{
	public interface IConnection
	{
		IConnectionState State { get; set; }

		byte[] Send(byte[] data);

		void SendAsync(byte[] data, Action<byte[]> responseCallback);

		void Disconnect();

		bool IsAlive();
	}
}
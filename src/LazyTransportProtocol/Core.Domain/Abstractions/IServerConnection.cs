using System;
using System.Collections.Generic;

namespace LazyTransportProtocol.Core.Domain.Abstractions
{
	public interface IServerConnection
	{
		byte[] Send(IList<ArraySegment<byte>> data);

		void Disconnect();

		bool IsAlive();
	}
}
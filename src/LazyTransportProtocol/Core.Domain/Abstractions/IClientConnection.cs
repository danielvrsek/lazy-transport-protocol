using System;
using System.Collections.Generic;

namespace LazyTransportProtocol.Core.Domain.Abstractions
{
	public interface IClientConnection
	{
		void Send(IList<ArraySegment<byte>> data);

		void Disconnect();
	}
}
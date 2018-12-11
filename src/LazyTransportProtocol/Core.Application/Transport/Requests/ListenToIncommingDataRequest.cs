using LazyTransportProtocol.Core.Application.Transport.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LazyTransportProtocol.Core.Application.Transport.Requests
{
	public delegate void ClientConnected(IClientConnection connection);

	public delegate void DataReceived(IClientConnection connection, byte[] data);

	public delegate void ErrorOccured(IClientConnection connection, Exception e);

	public class ListenToIncommingDataRequest : IRequest<ListenToIncommingDataResponse>
	{
		public IPAddress IPAddress { get; set; }

		public int Port { get; set; }

		public int BufferSize { get; set; } = 2048;
	}
}
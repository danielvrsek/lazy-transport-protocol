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
	public class ListenToIncommingDataRequest : IRequest<ListenToIncommingDataResponse>
	{
		public IPAddress IPAddress { get; set; }

		public int Port { get; set; }

		public int BufferSize { get; set; } = 2048;
	}
}
using LazyTransportProtocol.Core.Application.Transport.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace LazyTransportProtocol.Core.Application.Transport.Requests
{
	internal class SendDataRequest : IRequest<SendDataResponse>
	{
		public IList<ArraySegment<byte>> Data { get; set; }

		public Socket Sender { get; set; }
	}
}
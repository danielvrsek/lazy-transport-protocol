using LazyTransportProtocol.Core.Application.Transport.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using System.Net;

namespace LazyTransportProtocol.Core.Application.Transport.Requests
{
	internal class ListenToIncommingDataRequest : IRequest<ListenToIncommingDataResponse>
	{
		public IPAddress IPAddress { get; set; }

		public int Port { get; set; }

		public int BufferSize { get; set; } = 2048;
	}
}
using LazyTransportProtocol.Core.Application.Transport.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using System.Net;

namespace LazyTransportProtocol.Core.Application.Transport.Requests
{
	internal class ConnectToServerRequest : IRequest<ConnectToServerResponse>
	{
		public IPAddress IPAdress { get; set; }

		public int Port { get; set; }
	}
}
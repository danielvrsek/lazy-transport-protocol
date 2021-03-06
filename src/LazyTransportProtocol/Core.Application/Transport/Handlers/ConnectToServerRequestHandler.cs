using LazyTransportProtocol.Core.Application.Transport.Infrastructure;
using LazyTransportProtocol.Core.Application.Transport.Requests;
using LazyTransportProtocol.Core.Application.Transport.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Transport.Handlers
{
	internal class ConnectToServerRequestHandler : IRequestHandler<ConnectToServerRequest, ConnectToServerResponse>
	{
		public ConnectToServerResponse GetResponse(ConnectToServerRequest request)
		{
			IPEndPoint remoteEP = new IPEndPoint(request.IPAdress, request.Port);

			Socket sender = new Socket(request.IPAdress.AddressFamily,
				SocketType.Stream, ProtocolType.Tcp);

			sender.Connect(remoteEP);

			return new ConnectToServerResponse
			{
				Connection = new SocketServerConnection(sender)
			};
		}

		public Task<ConnectToServerResponse> GetResponseAsync(ConnectToServerRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
using LazyTransportProtocol.Core.Application.Transport.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Transport.Infrastructure;
using LazyTransportProtocol.Core.Application.Transport.Requests;
using LazyTransportProtocol.Core.Application.Transport.Responses;
using System.Net;
using System.Net.Sockets;

namespace LazyTransportProtocol.Core.Application.Transport.Handlers
{
	public class ConnectToServerRequestHandler : ITransportRequestHandler<ConnectToServerRequest, ConnectToServerResponse>
	{
		public ConnectToServerResponse GetResponse(ConnectToServerRequest request)
		{
			IPAddress ipAddress = IPAddress.Parse(request.IpAdress);
			IPEndPoint remoteEP = new IPEndPoint(ipAddress, request.Port);

			Socket sender = new Socket(ipAddress.AddressFamily,
				SocketType.Stream, ProtocolType.Tcp);

			sender.Connect(remoteEP);

			return new ConnectToServerResponse
			{
				Connection = new SocketConnection
				{
					Sender = sender
				}
			};
		}
	}
}
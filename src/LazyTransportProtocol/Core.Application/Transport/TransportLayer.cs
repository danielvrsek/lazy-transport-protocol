using LazyTransportProtocol.Core.Application.Transport.Requests;
using LazyTransportProtocol.Core.Application.Transport.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions;
using System.Net;

namespace LazyTransportProtocol.Core.Application.Transport
{
	public class TransportLayer : ITransport
	{
		private TransportRequestExecutor _transportExecutor = new TransportRequestExecutor();

		public IServerConnection Connect(IPAddress ipAdress, int port)
		{
			ConnectToServerResponse response = _transportExecutor.Execute(new ConnectToServerRequest
			{
				IPAdress = ipAdress,
				Port = port
			});

			return response.Connection;
		}

		public void Listen(IPAddress ipAddress, int port, ClientConnected clientConnected, DataReceived dataReceived, ErrorOccured errorOccured)
		{
			ListenToIncommingDataResponse response = _transportExecutor.Execute(new ListenToIncommingDataRequest
			{
				IPAddress = ipAddress,
				Port = port,
				BufferSize = 2048
			});

			response.ClientConnected += clientConnected;
			response.DataReceived += dataReceived;
			response.ErrorOccured += errorOccured;
		}
	}
}
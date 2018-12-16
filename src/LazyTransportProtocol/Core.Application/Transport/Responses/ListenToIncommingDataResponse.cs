using LazyTransportProtocol.Core.Application.Transport.Model;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;

namespace LazyTransportProtocol.Core.Application.Transport.Responses
{
	internal class ListenToIncommingDataResponse : IResponse
	{
		public event ClientConnected ClientConnected;

		public event DataReceived DataReceived;

		public event ErrorOccured ErrorOccured;

		internal void OnClientConnected(IClientConnection connection)
		{
			ClientConnected(connection);
		}

		internal void OnDataReceived(IClientConnection connection, byte[] data)
		{
			DataReceived(connection, data);
		}

		internal void OnErrorOccured(ErrorContext ctx)
		{
			ErrorOccured(ctx);
		}
	}
}
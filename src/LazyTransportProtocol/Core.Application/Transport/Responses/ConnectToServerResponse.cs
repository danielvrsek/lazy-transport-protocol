using LazyTransportProtocol.Core.Application.Transport.Abstractions.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions;

namespace LazyTransportProtocol.Core.Application.Transport.Responses
{
	public class ConnectToServerResponse : ITransportResponse
	{
		public IServerConnection Connection { get; set; }
	}
}
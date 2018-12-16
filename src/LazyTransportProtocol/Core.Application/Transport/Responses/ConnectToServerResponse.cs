using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;

namespace LazyTransportProtocol.Core.Application.Transport.Responses
{
	internal class ConnectToServerResponse : IResponse
	{
		public IServerConnection Connection { get; set; }
	}
}
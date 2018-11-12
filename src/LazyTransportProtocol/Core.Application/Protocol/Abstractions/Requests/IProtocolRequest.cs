using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Transport.Infrastructure;

namespace LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests
{
	public interface IProtocolRequest<TResponse> : IRequest<TResponse>
		where TResponse : class, IProtocolResponse, new()
	{
		string ProtocolCommand(ProtocolVersion protocolVersion);

		string Serialize(ProtocolVersion protocolVersion);

		string Deserialize(ProtocolVersion protocolVersion);
	}
}
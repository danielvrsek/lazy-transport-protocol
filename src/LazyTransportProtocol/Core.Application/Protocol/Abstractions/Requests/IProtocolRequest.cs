using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Transport.Infrastructure;
using System.Collections.Generic;

namespace LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests
{
	public interface IProtocolRequest<TResponse> : IRequest<TResponse>
		where TResponse : class, IProtocolResponse, new()
	{
		string GetIdentifier(ProtocolVersion protocolVersion);
	}
}
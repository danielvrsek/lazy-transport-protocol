using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Transport.Infrastructure;
using System;

namespace LazyTransportProtocol.Core.Application.Protocol.Requests
{
	public class ListDirectoryClientRequest : IProtocolRequest<ListDirectoryResponse>
	{
		public IConnection Connection => throw new NotImplementedException();

		public string Serialize(ProtocolVersion protocolVersion)
		{
			throw new NotImplementedException();
		}
	}
}
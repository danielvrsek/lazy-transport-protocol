using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Transport.Infrastructure;

namespace LazyTransportProtocol.Core.Application.Protocol.Requests
{
	public class AuthorizationRequest : IProtocolRequest<AuthorizationResponse>
	{
		public IConnection Connection => throw new System.NotImplementedException();

		public string Username { get; set; }

		public string Password { get; set; }

		public string Serialize(ProtocolVersion protocolVersion)
		{
			return $"LOGIN {Username} {Password}";
		}
	}
}
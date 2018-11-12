using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;

namespace LazyTransportProtocol.Core.Application.Protocol.Responses
{
	public class AuthorizationResponse : IProtocolResponse
	{
		public bool IsSuccess { get; set; }

		public string Serialize(ProtocolVersion protocolVersion)
		{
			throw new System.NotImplementedException();
		}
	}
}
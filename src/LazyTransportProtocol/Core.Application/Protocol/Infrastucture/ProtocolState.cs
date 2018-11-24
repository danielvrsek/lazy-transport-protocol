using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Infrastructure;
using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.Model;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Domain.Abstractions;

namespace LazyTransportProtocol.Core.Application.Infrastructure
{
	public class ProtocolState : IConnectionState
	{
		public AgreedHeadersDictionary AgreedHeaders { get; set; }

		public IAuthenticationContext AuthenticationContext { get; set; }
	}
}
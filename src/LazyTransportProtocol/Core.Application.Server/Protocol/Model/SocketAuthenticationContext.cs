using LazyTransportProtocol.Core.Application.Server.Protocol.Model;
using LazyTransportProtocol.Core.Application.Server.Protocol.Model.Abstraction.LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using System.Collections.Generic;

namespace LazyTransportProtocol.Core.Application.Protocol.Infrastucture
{
	internal class SocketAuthenticationContext : IAuthenticationContext
	{
		public List<Claim> Claims { get; set; } = new List<Claim>();
	}
}
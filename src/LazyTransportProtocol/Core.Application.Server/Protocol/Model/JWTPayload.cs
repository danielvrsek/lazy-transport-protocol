using System.Collections.Generic;

namespace LazyTransportProtocol.Core.Application.Server.Protocol.Model
{
	internal class JWTPayload
	{
		public List<Claim> Claims { get; set; } = new List<Claim>();
	}
}
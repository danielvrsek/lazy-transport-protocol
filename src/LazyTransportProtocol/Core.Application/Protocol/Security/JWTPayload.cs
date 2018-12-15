using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Security
{
	public class JWTPayload
	{
		public List<Claim> Claims { get; set; } = new List<Claim>();
	}
}

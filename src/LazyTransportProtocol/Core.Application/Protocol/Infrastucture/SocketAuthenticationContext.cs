using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Infrastucture
{
	public class SocketAuthenticationContext : IAuthenticationContext
	{
		public List<Claim> Claims { get; } = new List<Claim>();
	}
}

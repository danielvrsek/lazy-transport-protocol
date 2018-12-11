using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Infrastructure;
using LazyTransportProtocol.Core.Application.Protocol.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Infrastucture
{
	public class SocketAuthenticationContext : IAuthenticationContext
	{
		public List<Claim> Claims { get; set; } = new List<Claim>();
	}
}
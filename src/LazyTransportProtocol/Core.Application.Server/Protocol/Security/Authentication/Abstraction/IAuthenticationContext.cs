using LazyTransportProtocol.Core.Application.Server.Protocol.Model;
using System.Collections.Generic;

namespace LazyTransportProtocol.Core.Application.Server.Protocol.Security.Authentication.Abstraction
{
	internal interface IAuthenticationContext
	{
		List<Claim> Claims { get; }
	}
}
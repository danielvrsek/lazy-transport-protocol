using System.Collections.Generic;

namespace LazyTransportProtocol.Core.Application.Server.Protocol.Model.Abstraction
{
	namespace LazyTransportProtocol.Core.Application.Protocol.Infrastucture
	{
		internal interface IAuthenticationContext
		{
			List<Claim> Claims { get; }
		}
	}
}
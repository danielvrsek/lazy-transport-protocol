using LazyTransportProtocol.Core.Application.Protocol.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Abstractions.Infrastructure
{
    public interface IAuthenticationContext
    {
		List<Claim> Claims { get; }
    }
}
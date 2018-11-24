using LazyTransportProtocol.Core.Application.Protocol.Metadata;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Extensions
{
    public static class ClaimListExtenstions
    {
		public static string GetUsername(this List<Claim> claims)
		{
			Claim claim = claims.Find((x) => x.Type == ClaimTypesMetadata.Username);

			return claim?.Value;
		}
	}
}

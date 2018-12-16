using LazyTransportProtocol.Core.Application.Protocol.Requests.Abstractions;
using LazyTransportProtocol.Core.Application.Protocol.Responses.Abstractions;
using LazyTransportProtocol.Core.Application.Server.Protocol.Model;
using LazyTransportProtocol.Core.Application.Server.Protocol.Security.Authentication.Abstraction;
using System.Collections.Generic;

namespace LazyTransportProtocol.Core.Application.Server.Protocol.Security.Authentication
{
	internal class AuthenticationContext : IAuthenticationContext
	{
		private static readonly object _lock = new object();

		private static readonly Dictionary<int, AuthenticationContext> _authenticationContexts = new Dictionary<int, AuthenticationContext>();

		public static void Insert<TResponse>(IProtocolRequest<TResponse> request, AuthenticationContext authenticationContext)
			where TResponse : IProtocolResponse
		{
			lock (_lock)
			{
				_authenticationContexts.Add(request.GetHashCode(), authenticationContext);
			}
		}

		public static AuthenticationContext Get<TResponse>(IProtocolRequest<TResponse> request)
			where TResponse : IProtocolResponse
		{
			lock (_lock)
			{
				if (_authenticationContexts.TryGetValue(request.GetHashCode(), out AuthenticationContext context))
				{
					return context;
				}

				return null;
			}
		}

		public List<Claim> Claims { get; set; } = new List<Claim>();
	}
}
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Infrastructure;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Requests
{
	public abstract class AuthenticatedRequest<TResponse> : IAuthenticatedRequest<TResponse>
		where TResponse : IProtocolResponse
	{
		[JsonIgnore]
		public IAuthenticationContext AuthenticationContext { get; set; }

		public string AuthenticationToken { get; set; }

		public abstract string GetIdentifier();
	}
}
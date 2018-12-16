using LazyTransportProtocol.Core.Application.Protocol.Requests.Abstractions;
using LazyTransportProtocol.Core.Application.Server.Protocol.Responses;
using System;

namespace LazyTransportProtocol.Core.Application.Server.Protocol.Requests
{
	internal class ValidateAuthenticationRequest : IProtocolRequest<ValidateAuthenticationResponse>
	{
		public string AuthenticationToken { get; set; }

		public string GetIdentifier()
		{
			throw new NotImplementedException();
		}
	}
}
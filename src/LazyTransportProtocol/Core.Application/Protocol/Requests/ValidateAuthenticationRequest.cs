using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Requests
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
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Responses
{
	internal class ValidateAuthenticationResponse : AcknowledgementResponse
	{
		public List<Claim> Claims { get; internal set; }
	}
}
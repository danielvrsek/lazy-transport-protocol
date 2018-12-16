using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Server.Protocol.Model;
using System.Collections.Generic;

namespace LazyTransportProtocol.Core.Application.Server.Protocol.Responses
{
	internal class ValidateAuthenticationResponse : AcknowledgementResponse
	{
		public List<Claim> Claims { get; internal set; }
	}
}
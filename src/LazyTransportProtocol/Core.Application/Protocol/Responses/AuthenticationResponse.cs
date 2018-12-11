using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Responses
{
	public class AuthenticationResponse : AcknowledgementResponse
	{
		public string AuthenticationToken { get; set; }
	}
}
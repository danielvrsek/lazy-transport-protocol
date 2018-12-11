using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Infrastructure;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Requests
{
	public class ModifyUserRequest : IProtocolRequest<AcknowledgementResponse>
	{
		public const string Identifier = "USERMODIFY";

		public string Username { get; set; }

		public string Password { get; set; }

		public IAuthenticationContext AuthenticationContext { get; set; }

		public string GetIdentifier()
		{
			return Identifier;
		}
	}
}

using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Responses
{
	public class ErrorResponse : IProtocolResponse
	{
		public const string Identifier = "ERRRESP";

		public int Code { get; set; }

		public string Message { get; set; }

		public string GetIdentifier()
		{
			return Identifier;
		}
	}
}

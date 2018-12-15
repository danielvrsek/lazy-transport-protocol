using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Responses
{
	public class AcknowledgementResponse : IProtocolResponse
	{
		public const string Identifier = "ACKRESP";

		public int Code { get; set; }

		public bool IsSuccessful
		{
			get
			{
				return Code == 200;
			}
		}

		public virtual string GetIdentifier()
		{
			return Identifier;
		}
	}
}
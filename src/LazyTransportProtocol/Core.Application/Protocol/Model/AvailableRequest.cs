using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Model
{
	public struct AvailableRequest
	{
		public string Identifier { get; }

		public Type RequestType { get; }

		public AvailableRequest(string identifier, Type requestType)
		{
			Identifier = identifier;
			RequestType = requestType;
		}
	}
}
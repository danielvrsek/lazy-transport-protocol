using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Security
{
	public class Claim
	{
		public Claim(string type, string value)
		{
			Type = type;
			Value = value;
		}

		public string Type { get; }

		public string Value { get; }
	}
}
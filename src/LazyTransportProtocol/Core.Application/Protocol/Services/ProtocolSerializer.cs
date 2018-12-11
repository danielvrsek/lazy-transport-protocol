using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class ProtocolSerializer
	{
		private const string separator = ";";

		public static string Serialize(string identifier, string headers, string body)
		{
			return identifier + separator + headers + separator + body;
		}
	}
}
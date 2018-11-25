using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class ProtocolSerializer
	{
		public static string Serialize(string identifier, string headers, string body, AgreedHeadersDictionary agreedHeaders)
		{
			return Serialize(identifier, headers, body, agreedHeaders, agreedHeaders.ProtocolVersion);
		}

		public static string Serialize(string identifier, string headers, string body, AgreedHeadersDictionary agreedHeaders, ProtocolVersion protocolVersion)
		{
			return identifier + agreedHeaders.Separator + headers + agreedHeaders.Separator + body;
		}
	}
}
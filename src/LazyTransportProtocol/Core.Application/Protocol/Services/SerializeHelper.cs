using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class SerializeHelper
	{
		public static string SerializeRequestString(string identifier, string body, AgreedHeadersDictionary headers)
		{
			return SerializeRequestString(identifier, body, headers, headers.ProtocolVersion);
		}

		public static string SerializeRequestString(string identifier, string body, AgreedHeadersDictionary headers, ProtocolVersion protocolVersion)
		{
			return identifier + headers.Separator + body;
		}
	}
}
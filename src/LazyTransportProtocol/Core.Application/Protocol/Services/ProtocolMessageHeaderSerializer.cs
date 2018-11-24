using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class ProtocolMessageHeaderSerializer
	{
		public string Serialize(MessageHeadersDictionary requestHeaders, ProtocolVersion protocolVersion)
		{
			StringBuilder sb = new StringBuilder();

			foreach (KeyValuePair<string, string> header in requestHeaders)
			{
				if (sb.Length != 0)
				{
					sb.Append('&');
				}

				sb.Append($"{header.Key}={header.Value}");
			}

			return sb.ToString();
		}
	}
}

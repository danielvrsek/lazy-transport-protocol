using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class ProtocolMessageHeaderDeserializer
	{
		public MessageHeadersDictionary Deserialize(string headersString, ProtocolVersion protocolVersion)
		{
			NameValueCollection collection = HttpUtility.ParseQueryString(headersString);

			MessageHeadersDictionary headers = new MessageHeadersDictionary();

			foreach (string key in collection.AllKeys)
			{
				headers[key] = collection[key];
			}

			return headers;
		}
	}
}

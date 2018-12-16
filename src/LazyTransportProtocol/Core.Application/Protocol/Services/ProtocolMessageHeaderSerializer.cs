using LazyTransportProtocol.Core.Application.Protocol.Model;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class ProtocolMessageHeaderSerializer
	{
		public static string Serialize(MessageHeadersDictionary requestHeaders)
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
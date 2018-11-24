using LazyTransportProtocol.Core.Application.Protocol.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Infrastucture
{
	public class MessageHeadersDictionary : Dictionary<string, string>
	{
		public bool IsChunked
		{
			get
			{
				return ContainsKey(MessageHeadersMetadata.Chunk);
			}
		}

		public string Chunk
		{
			get
			{
				return this[MessageHeadersMetadata.Chunk];
			}
		}

		public bool IsRequestPart
		{
			get
			{
				return ContainsKey(MessageHeadersMetadata.RequestPart);
			}
		}

		public string RequestPart
		{
			get
			{
				return this[MessageHeadersMetadata.RequestPart];
			}
		}

		public bool ContainsAuthorizationToken
		{
			get
			{
				return ContainsKey(MessageHeadersMetadata.AuthentizationToken);
			}
		}

		public string AuthorizationToken
		{
			get
			{
				return this[MessageHeadersMetadata.AuthentizationToken];
			}
		}
	}
}
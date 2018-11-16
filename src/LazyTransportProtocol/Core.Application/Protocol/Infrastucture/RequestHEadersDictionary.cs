using LazyTransportProtocol.Core.Application.Protocol.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Infrastucture
{
	public class RequestHeadersDictionary : Dictionary<string, string>
	{
		public bool IsChunked
		{
			get
			{
				return ContainsKey(RequestHeadersMetadata.Chunk);
			}
	}
}
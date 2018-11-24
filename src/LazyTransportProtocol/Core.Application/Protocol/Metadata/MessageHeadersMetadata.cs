using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Metadata
{
	public static class MessageHeadersMetadata
	{
		public const string Chunk = "chunk";
		public const string AuthentizationToken = "authorization-token";
		internal const string RequestPart = "request-part";
	}
}
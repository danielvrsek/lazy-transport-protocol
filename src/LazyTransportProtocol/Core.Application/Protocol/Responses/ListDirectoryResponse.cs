using LazyTransportProtocol.Core.Application.Protocol.Model;
using LazyTransportProtocol.Core.Application.Protocol.Responses.Abstractions;
using System.Collections.Generic;

namespace LazyTransportProtocol.Core.Application.Protocol.Responses
{
	public class ListDirectoryResponse : IProtocolResponse
	{
		public const string Identifier = "LISTRESP";

		public List<RemoteFile> RemoteFiles { get; set; }

		public List<RemoteDirectory> RemoteDirectories { get; set; }

		public string GetIdentifier()
		{
			return Identifier;
		}
	}
}
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Model;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Responses
{
	public class ListDirectoryResponse : IProtocolResponse
	{
		public const string Identifier = "LIST";

		public List<RemoteFile> RemoteFiles { get; set; }

		public List<RemoteDirectory> RemoteDirectories { get; set; }

		public string GetIdentifier(ProtocolVersion protocolVersion)
		{
			return Identifier;
		}
	}
}
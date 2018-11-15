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
		public List<RemoteFile> RemoteFiles { get; set; }

		public List<RemoteDirectory> RemoteDirectories { get; set; }

		public string Serialize(ProtocolVersion protocolVersion)
		{
			string directoriesSerialized =
		}

		public void Deserialize(string data, ProtocolVersion protocolVersion)
		{
			throw new NotImplementedException();
		}
	}
}
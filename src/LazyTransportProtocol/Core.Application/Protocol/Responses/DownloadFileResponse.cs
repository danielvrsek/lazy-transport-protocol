using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Responses
{
	public class DownloadFileResponse : IProtocolResponse
	{
		public const string Identifier = "DOWNFILERESP";

		public byte[] Data { get; set; }

		public bool HasNext { get; set; }

		public string GetIdentifier()
		{
			return Identifier;
		}
	}
}
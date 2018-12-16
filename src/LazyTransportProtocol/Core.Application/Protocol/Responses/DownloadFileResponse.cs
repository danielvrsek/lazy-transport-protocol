using LazyTransportProtocol.Core.Application.Protocol.Responses.Abstractions;

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
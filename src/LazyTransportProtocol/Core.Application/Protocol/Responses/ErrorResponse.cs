using LazyTransportProtocol.Core.Application.Protocol.Responses.Abstractions;

namespace LazyTransportProtocol.Core.Application.Protocol.Responses
{
	public class ErrorResponse : IProtocolResponse
	{
		public const string Identifier = "ERRRESP";

		public int Code { get; set; }

		public string Message { get; set; }

		public string GetIdentifier()
		{
			return Identifier;
		}
	}
}
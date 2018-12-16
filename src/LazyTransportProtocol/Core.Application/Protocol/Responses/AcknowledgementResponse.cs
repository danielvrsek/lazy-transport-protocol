using LazyTransportProtocol.Core.Application.Protocol.Responses.Abstractions;

namespace LazyTransportProtocol.Core.Application.Protocol.Responses
{
	public class AcknowledgementResponse : IProtocolResponse
	{
		public const string Identifier = "ACKRESP";

		public int Code { get; set; }

		public bool IsSuccessful
		{
			get
			{
				return Code == 200;
			}
		}

		public virtual string GetIdentifier()
		{
			return Identifier;
		}
	}
}
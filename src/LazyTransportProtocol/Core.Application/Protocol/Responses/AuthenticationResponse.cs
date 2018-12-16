namespace LazyTransportProtocol.Core.Application.Protocol.Responses
{
	public class AuthenticationResponse : AcknowledgementResponse
	{
		public string AuthenticationToken { get; set; }

		public override string GetIdentifier()
		{
			return "AUTHRESP";
		}
	}
}
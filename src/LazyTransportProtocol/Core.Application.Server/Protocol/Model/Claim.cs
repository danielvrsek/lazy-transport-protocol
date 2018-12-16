namespace LazyTransportProtocol.Core.Application.Server.Protocol.Model
{
	internal class Claim
	{
		public Claim(string type, string value)
		{
			Type = type;
			Value = value;
		}

		public string Type { get; }

		public string Value { get; }
	}
}
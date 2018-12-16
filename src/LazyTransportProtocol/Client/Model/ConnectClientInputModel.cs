namespace LazyTransportProtocol.Client.Model
{
	public class ConnectClientInputModel
	{
		public string IpAddress { get; set; }

		public string Port { get; set; }

		public bool ForceLogin { get; set; }
	}
}
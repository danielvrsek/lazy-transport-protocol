namespace LazyTransportProtocol.Core.Application.Server.Configuration
{
	public class ServerConfiguration
	{
		private readonly static object _lock = new object();
		private static ServerConfiguration _instance = null;

		public static ServerConfiguration Instance()
		{
			lock (_lock)
			{
				if (_instance == null)
				{
					_instance = new ServerConfiguration();
				}
			}

			return _instance;
		}

		private ServerConfiguration()
		{
		}

		public string RootFolder = "LTP";

		public string UserSecretFilepath = "secrets";

		public string ServerSecret = null;
	}
}
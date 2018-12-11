using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Configuration
{
	public class ServerConfiguration
	{
		private static object _lock = new object();
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
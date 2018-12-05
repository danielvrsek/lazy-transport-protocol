using LazyTransportProtocol.Core.Application.Protocol.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LazyTransportProtocol.Server
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			if (args.Length < 2)
			{
				Console.WriteLine("IP adress and/or port missing.");
				return;
			}

			if (!IPAddress.TryParse(args[0], out IPAddress ipAddress))
			{
				Console.WriteLine("Invalid ip address.");
				Console.ReadLine();
				return;
			}

			if (!Int32.TryParse(args[1], out int port))
			{
				Console.WriteLine("Invalid port number.");
				Console.ReadLine();
				return;
			}

			LoadConfig("config.cfg");

			ProtocolRequestListener listener = new ProtocolRequestListener();
			listener.Listen(ipAddress, port);
		}

		private static void LoadConfig(string configPath)
		{
			var config = new
			{
				ServerConfiguration.RootFolder,
				ServerConfiguration.UserSecretFilepath
			};

			try
			{
				if (!File.Exists(configPath))
				{
					string json = JsonConvert.SerializeObject(config);

					using (StreamWriter sw = new StreamWriter(configPath))
					{
						sw.Write(json);
					}
				}
				else
				{
					using (StreamReader sr = new StreamReader(configPath))
					{
						var loadedConfig = JsonConvert.DeserializeAnonymousType(sr.ReadToEnd(), config);

						ServerConfiguration.RootFolder = loadedConfig.RootFolder;
						ServerConfiguration.UserSecretFilepath = loadedConfig.UserSecretFilepath;
					}
				}
			}
			catch
			{
				Console.WriteLine("Configuration could not be loaded. Using default values.");
			}
		}
	}
}
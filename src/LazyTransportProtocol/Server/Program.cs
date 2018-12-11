using LazyTransportProtocol.Core.Application.Protocol.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
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
			ServerConfiguration serverConfig = ServerConfiguration.Instance();
			serverConfig.ServerSecret = RandomString(16);

			try
			{
				if (!File.Exists(configPath))
				{
					string json = JsonConvert.SerializeObject(serverConfig);

					using (StreamWriter sw = new StreamWriter(configPath))
					{
						sw.Write(json);
					}
				}
				else
				{
					ServerConfiguration loadedConfig;
					using (StreamReader sr = new StreamReader(configPath))
					{
						loadedConfig = JsonConvert.DeserializeObject<ServerConfiguration>(sr.ReadToEnd());

						serverConfig.RootFolder = loadedConfig.RootFolder;
						serverConfig.UserSecretFilepath = loadedConfig.UserSecretFilepath;
						serverConfig.ServerSecret = loadedConfig.ServerSecret;
					}

					// Update with "new" values
					string json = JsonConvert.SerializeObject(serverConfig);

					using (StreamWriter sw = new StreamWriter(configPath))
					{
						sw.Write(json);
					}
				}
			}
			catch
			{
				Console.WriteLine("Configuration could not be loaded. Using default values.");
			}
		}

		private static string RandomString(int length)
		{
			Random random = new Random();
			const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, length)
			  .Select(s => s[random.Next(s.Length)]).ToArray());
		}
	}
}
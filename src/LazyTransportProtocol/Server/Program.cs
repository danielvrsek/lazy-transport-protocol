using fastJSON;
using LazyTransportProtocol.Core.Application.Server.Configuration;
using LazyTransportProtocol.Core.Application.Server.Services;
using LazyTransportProtocol.Core.Application.Server.Services.Abstractions;
using System;
using System.IO;
using System.Linq;
using System.Net;

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
			try
			{
				LoadConfig("config.cfg");

				CreateFileSecretIfNotExist();

				CheckIfRootFolderExists();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine("Exiting...");
				return;
			}

			ProtocolRequestListener listener = new ProtocolRequestListener();
			listener.Listen(ipAddress, port);
		}

		private static void CheckIfRootFolderExists()
		{
			ServerConfiguration serverConfig = ServerConfiguration.Instance();

			if (!Directory.Exists(serverConfig.RootFolder))
			{
				Directory.CreateDirectory(serverConfig.RootFolder);
				//throw new Exception("Root folder must exist.");
			}
		}

		private static void CreateFileSecretIfNotExist()
		{
			ServerConfiguration serverConfig = ServerConfiguration.Instance();

			if (!File.Exists(serverConfig.UserSecretFilepath))
			{
				string directoryName = Path.GetDirectoryName(serverConfig.UserSecretFilepath);
				if (!String.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}

				using (StreamWriter file = new StreamWriter(serverConfig.UserSecretFilepath))
				{
					file.WriteLine("#########");
				}

				IUserService userService = new UserService();
				userService.CreateNew("admin", "admin");
			}
		}

		private static void LoadConfig(string configPath)
		{
			ServerConfiguration serverConfig = ServerConfiguration.Instance();
			serverConfig.ServerSecret = RandomString(35);

			try
			{
				if (!File.Exists(configPath))
				{
					string json = JSON.ToJSON(serverConfig);

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
						loadedConfig = JSON.ToObject<ServerConfiguration>(sr.ReadToEnd());

						serverConfig.RootFolder = loadedConfig.RootFolder ?? serverConfig.RootFolder;
						serverConfig.UserSecretFilepath = loadedConfig.UserSecretFilepath ?? serverConfig.UserSecretFilepath;
						serverConfig.ServerSecret = loadedConfig.ServerSecret ?? serverConfig.ServerSecret;
					}

					// Update with "new" values
					string json = JSON.ToJSON(serverConfig);

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
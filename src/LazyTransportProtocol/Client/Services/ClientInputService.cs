using LazyTransportProtocol.Client.Exceptions;
using LazyTransportProtocol.Client.Metadata;
using LazyTransportProtocol.Client.Model;
using LazyTransportProtocol.Core.Application.Protocol.Flow;
using LazyTransportProtocol.Core.Domain.Exceptions.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LazyTransportProtocol.Client.Services
{
	public class ClientInputService
	{
		private readonly Dictionary<string, Action<string[]>> _commandDictionary = new Dictionary<string, Action<string[]>>();

		private readonly ClientFlowService _clientFlowService = new ClientFlowService();

		public ClientInputService()
		{
			
				

			_commandDictionary[CommandNameMetadata.Download] = ArgumentCondition.Or(
					ArgumentCondition.Or(
						new Argument<DownloadFileClientInputModel>("l", (param, model) => model.LocalFile = param),
						new Argument<DownloadFileClientInputModel>("local", (param, model) => model.LocalFile = param)),
					new Argument<DownloadFileClientInputModel>(0, (param, model) => model.LocalFile = param)
						.PromtIfEmpty("Local file"));
				.Execute;

			_commandDictionary[CommandNameMetadata.Connect] = ConnectHandler;
			_commandDictionary[CommandNameMetadata.Authenticate] = AuthenticateHandler;
			_commandDictionary[CommandNameMetadata.User] = UserHandler;
			//_commandDictionary[CommandNameMetadata.Download] = DownloadFileHandler;
			_commandDictionary[CommandNameMetadata.ListDirectory] = ListDirectory;

			
		}

		public void Execute(string commandRequest)
		{
			string[] flags = commandRequest.Split(' ');

			string command = flags[0];

			if (!_commandDictionary.ContainsKey(command))
			{
				throw new CommandException("Invalid command.");
			}

			string[] parameters = flags.Skip(1)
				.Where(x => !String.IsNullOrWhiteSpace(x))
				.Select(x =>
				{
					if (x.StartsWith('"') && x.EndsWith('"'))
					{
						return x.Substring(1, x.Length - 2);
					}

					return x;
				}).ToArray();

			try
			{
				_commandDictionary[command](parameters);
			}
			catch (ConnectionRequiredException)
			{
				throw new CommandException("Connection to the remote host is required.");
			}
		}

		private void ConnectHandler(string[] parameters)
		{
			string ipString = null;
			string portString = null;

			if (parameters.Length > 0)
			{
				ipString = parameters[0];
			}
			else
			{
				Console.Write("Remote host IP: ");
				ipString = Console.ReadLine();
			}

			// Deserialize IP 
			if (ipString.Contains(':'))
			{
				string[] flags = ipString.Split(':');

				if (String.IsNullOrWhiteSpace(flags[1]))
				{
					throw new CommandException("Port number missing.");
				}

				ipString = flags[0];
				portString = flags[1];
			}
			else
			{
				if (parameters.Length >= 1)
				{
					portString = parameters[1];
				}
				else
				{
					Console.WriteLine("Port: ");
					portString = Console.ReadLine();
				}
			}

			if (String.IsNullOrWhiteSpace(ipString))
			{
				throw new CommandException("IP adress cannot be empty.");
			}

			if (!Int32.TryParse(portString, out int port))
			{
				throw new CommandException("Could not parse port number.");
			}

			Console.WriteLine($"Connecting to remote host {ipString} at {portString} ...");
			_clientFlowService.Connect(ipString, port);

			Execute(CommandNameMetadata.Authenticate);
		}

		private void AuthenticateHandler(string[] parameters)
		{
			string username = null;
			string password = null;

			GetUsernameAndPassword(parameters, ref username, ref password);

			if (String.IsNullOrWhiteSpace(username) || String.IsNullOrWhiteSpace(password))
			{
				throw new CommandException("Username and password cannot be empty.");
			}

			Console.WriteLine("Authenticating user " + username + "...");
			while (!_clientFlowService.Authenticate(username, password))
			{
				// Force new credetials
				GetUsernameAndPassword(new string[0], ref username, ref password);
			}

			Console.WriteLine("Authentication successful.");
		}

		private void GetUsernameAndPassword(string[] parameters, ref string username, ref string password)
		{
			if (parameters.Length > 0)
			{
				username = parameters[0];
			}
			else
			{
				Console.Write("Username: ");
				username = Console.ReadLine();
			}

			if (parameters.Length > 1)
			{
				password = parameters[1];
			}
			else
			{
				Console.Write("Password: ");
				password = ReadSecureString();
			}
		}

		private void UserHandler(string[] parameters)
		{
			if (parameters.Length == 0)
			{
				throw new CommandException("Insufficient arguments.");
			}

			if (parameters[0] == "create")
			{
				CreateUserHandler(parameters.Skip(1).ToArray());
			}
			else if (parameters[0] == "delete")
			{
				DeleteUserHandler(parameters.Skip(1).ToArray());
			}
			else
			{
				throw new CommandException("Unrecognized argument.");
			}
		}

		private void CreateUserHandler(string[] parameters)
		{

		}

		private void DeleteUserHandler(string[] parameters)
		{

		}

		private void DownloadFileHandler(string[] parameters)
		{
			string remoteFilepath = parameters[0];
			string localFilepath = parameters[1];

			_clientFlowService.DownloadFile(remoteFilepath, localFilepath);
		}

		private void ListDirectory(string[] parameters)
		{
			string path;

			if (parameters.Length > 0)
			{
				path = parameters[0];
			}
			else
			{
				throw new CommandException("Insufficient parameters.");
			}

			_clientFlowService.ListDirectory(path);
		}

		private void ChangeDirectory(string[] parameters)
		{
			string path;

			if (parameters.Length > 0)
			{
				path = parameters[0];
			}
			else
			{
				throw new CommandException("Insufficient parameters.");
			}

			_clientFlowService.ListDirectory(path);
		}

		private string ReadSecureString()
		{
			StringBuilder sb = new StringBuilder();

			ConsoleKeyInfo key;

			while (true)
			{
				key = Console.ReadKey(true);

				if (key.Key == ConsoleKey.Enter)
				{
					Console.WriteLine();
					break;
				}
				else if (key.Key == ConsoleKey.Backspace)
				{
					if (sb.Length > 0)
					{
						Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
						Console.Write(' ');
						Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);

						if (sb.Length > 1)
						{
							sb.Remove(sb.Length - 2, 1);
						}
						else
						{
							sb.Remove(0, 1);
						}
					}
				}
				else
				{
					Console.Write('*');
					sb.Append(key.KeyChar);
				}
			}

			return sb.ToString();
		}
	}
}

using LazyTransportProtocol.Client.Exceptions;
using LazyTransportProtocol.Client.Helpers;
using LazyTransportProtocol.Client.Metadata;
using LazyTransportProtocol.Client.Model;
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

		public string Host => _clientFlowService.Host;

		public string Username => _clientFlowService.Username;

		public string CurrentFolder => _clientFlowService?.CurrentFolder;

		public ClientInputService()
		{
			_commandDictionary[CommandNameMetadata.Download] = new ArgumentClientInput<DownloadFileClientInputModel>(model => _clientFlowService.DownloadFile(model.RemoteFile, model.LocalFile))
				.RegisterArgument(
					ArgumentCondition.Or(
						Argument.Create<DownloadFileClientInputModel>((param, model) => model.RemoteFile = param, "-r", "--remote"),
						Argument.Create<DownloadFileClientInputModel>((param, model) => model.RemoteFile = param, 0)
							.PromtIfEmpty("Remote file")))
				.RegisterArgument(
					ArgumentCondition.Or(
						Argument.Create<DownloadFileClientInputModel>((param, model) => model.LocalFile = param, "-l", "--local"),
						Argument.Create<DownloadFileClientInputModel>((param, model) => model.LocalFile = param, 1)
							.PromtIfEmpty("Local file"))).Execute;

			_commandDictionary[CommandNameMetadata.ChangeDirectory] = new ArgumentClientInput<ChangeDirectoryClientInputModel>(model => _clientFlowService.ChangeDirectory(model.Path))
				.RegisterArgument(
					ArgumentCondition.Or(
						Argument.Create<ChangeDirectoryClientInputModel>((param, model) => model.Path = param, "-p", "--path"),
						Argument.Create<ChangeDirectoryClientInputModel>((param, model) => model.Path = param, 0))).Execute;

			_commandDictionary[CommandNameMetadata.ListDirectory] = new ArgumentClientInput<ListDirectoryClientInputModel>(model => _clientFlowService.ListDirectory(model.Path))
				.RegisterArgument(
					ArgumentCondition.Or(
						Argument.Create<ListDirectoryClientInputModel>((param, model) => model.Path = param, "-p", "--path"),
						Argument.Create<ListDirectoryClientInputModel>((param, model) => model.Path = param, 0)
							.SetDefaultValue(""))).Execute;

			_commandDictionary[CommandNameMetadata.Connect] = new ArgumentClientInput<ConnectClientInputModel>(ConnectHandler)
				.RegisterArgument(
					ArgumentCondition.Or(
						Argument.Create<ConnectClientInputModel>((param, model) => model.IpAddress = param, "-ip"),
						Argument.Create<ConnectClientInputModel>((param, model) => model.IpAddress = param, 0)
							.PromtIfEmpty("Remote host")))
				.RegisterArgument(
					ArgumentCondition.Or(
						Argument.Create<ConnectClientInputModel>((param, model) => model.Port = param, "-p"),
						Argument.Create<ConnectClientInputModel>((param, model) => model.Port = param, 1)
							.PromtIfEmpty("Remote port")))
				.RegisterArgument(Option.Create<ConnectClientInputModel>((param, model) => model.ForceLogin = !param, "--no-login")).Execute;

			_commandDictionary[CommandNameMetadata.Upload] = new ArgumentClientInput<UploadFileClientInputModel>(model => _clientFlowService.UploadFile(model.LocalFile, model.RemoteFile))
				.RegisterArgument(
					ArgumentCondition.Or(
						Argument.Create<UploadFileClientInputModel>((param, model) => model.LocalFile = param, "-l", "--local"),
						Argument.Create<UploadFileClientInputModel>((param, model) => model.LocalFile = param, 0)
							.PromtIfEmpty("Local file")))
				.RegisterArgument(
					ArgumentCondition.Or(
						Argument.Create<UploadFileClientInputModel>((param, model) => model.RemoteFile = param, "-r", "--remote"),
						Argument.Create<UploadFileClientInputModel>((param, model) => model.RemoteFile = param, 1)
							.PromtIfEmpty("Remote file"))).Execute;

			_commandDictionary[CommandNameMetadata.CreateDirectory] = new ArgumentClientInput<CreateDirectoryClientInputModel>(model => _clientFlowService.CreateDirectory(model.FullPath))
				.RegisterArgument(Argument.Create<CreateDirectoryClientInputModel>((param, model) => model.FullPath = param, 0)).Execute;

			_commandDictionary[CommandNameMetadata.Authenticate] = AuthenticateHandler;
			_commandDictionary[CommandNameMetadata.User] = UserHandler;
		}

		public bool Execute(string commandRequest)
		{
			List<string> flags = ParseArguments(commandRequest);
			string command = flags[0];

			if (command == "exit")
			{
				_clientFlowService.Disconnect();
				return false;
			}

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
				return true;
			}
			catch (ConnectionRequiredException)
			{
				throw new CommandException("Connection to the remote host is required.");
			}
		}

		private void ConnectHandler(ConnectClientInputModel model)
		{
			if (String.IsNullOrWhiteSpace(model.IpAddress))
			{
				throw new CommandException("IP adress cannot be empty.");
			}

			if (!Int32.TryParse(model.Port, out int port))
			{
				throw new CommandException("Could not parse port number.");
			}

			Console.WriteLine($"Connecting to remote host {model.IpAddress} at {model.Port} ...");
			_clientFlowService.Connect(model.IpAddress, port);

			if (model.ForceLogin)
			{
				Execute(CommandNameMetadata.Authenticate + " --force");
			}
		}

		private void AuthenticateHandler(string[] parameters)
		{
			string username = null;
			string password = null;
			bool force = parameters.Contains("--force");

			GetUsernameAndPassword(parameters, ref username, ref password);

			Console.WriteLine("Authenticating user " + username + "...");

			bool isSuccess;
			while (!(isSuccess = _clientFlowService.Authenticate(username, password)) && force)
			{
				Console.WriteLine("Authentication failed.");

				// Force new credetials
				GetUsernameAndPassword(new string[0], ref username, ref password);
			}

			if (isSuccess)
			{
				Console.WriteLine("Authentication successful.");
			}
			else
			{
				Console.WriteLine("Authentication failed.");
			}
		}

		private void GetUsernameAndPassword(string[] parameters, ref string username, ref string password)
		{
			if (parameters.Length > 0 && !parameters[0].StartsWith('-'))
			{
				username = parameters[0];
			}
			else
			{
				Console.Write("Username: ");
				username = Console.ReadLine();
			}

			if (parameters.Length > 1 && !parameters[1].StartsWith('-'))
			{
				password = parameters[1];
			}
			else
			{
				Console.Write("Password: ");
				password = ConsoleHelper.ReadSecureString();
			}

			if (String.IsNullOrWhiteSpace(username) || String.IsNullOrWhiteSpace(password))
			{
				throw new CommandException("Username and password cannot be empty.");
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
				new ArgumentClientInput<CreateUserClientInputModel>(model => _clientFlowService.CreateNewUser(model.Username, model.Password))
				.RegisterArgument(
					ArgumentCondition.Or(
						Argument.Create<CreateUserClientInputModel>((param, model) => model.Username = param, "-u", "--username"),
						Argument.Create<CreateUserClientInputModel>((param, model) => model.Username = param, 0)
							.PromtIfEmpty("Username")))
				.RegisterArgument(
					ArgumentCondition.Or(
						Argument.Create<CreateUserClientInputModel>((param, model) => model.Password = param, "-p", "--password"),
						Argument.Create<CreateUserClientInputModel>((param, model) => model.Password = param, 1)
							.PromtIfEmpty("Password", true)))
				.Execute(parameters.Skip(1).ToArray());
			}
			else if (parameters[0] == "delete")
			{
				new ArgumentClientInput<DeleteUserClientInputModel>(model => _clientFlowService.DeleteUser(model.Username))
				.RegisterArgument(
					ArgumentCondition.Or(
						Argument.Create<DeleteUserClientInputModel>((param, model) => model.Username = param, "-u", "--username"),
						Argument.Create<DeleteUserClientInputModel>((param, model) => model.Username = param, 0)
							.PromtIfEmpty("Username")))
				.Execute(parameters.Skip(1).ToArray());
			}
			else
			{
				throw new CommandException("Unrecognized argument.");
			}
		}

		private List<string> ParseArguments(string command)
		{
			StringBuilder sb = new StringBuilder();

			List<string> flags = new List<string>();

			bool isQuoted = false;

			for (int i = 0; i < command.Length; i++)
			{
				if (command[i] == ' ' && !isQuoted)
				{
					flags.Add(sb.ToString());
					sb.Clear();
				}
				else
				{
					if (command[i] == '"')
					{
						isQuoted = !isQuoted;
					}

					sb.Append(command[i]);
				}
			}

			flags.Add(sb.ToString());

			return flags;
		}
	}
}
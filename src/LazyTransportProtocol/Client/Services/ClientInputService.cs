using LazyTransportProtocol.Client.Exceptions;
using LazyTransportProtocol.Client.Helpers;
using LazyTransportProtocol.Client.Metadata;
using LazyTransportProtocol.Client.Model;
using LazyTransportProtocol.Core.Application.Protocol.Flow;
using LazyTransportProtocol.Core.Domain.Exceptions;
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
			_commandDictionary[CommandNameMetadata.Download] = new ArgumentClientInput<DownloadFileClientInputModel>(model => _clientFlowService.DownloadFile(model.RemoteFile, model.LocalFile))
				.RegisterArgument(
					ArgumentCondition.Or(
						Argument.Create<DownloadFileClientInputModel>((param, model) => model.RemoteFile = param, "-r", "-remote"),
						Argument.Create<DownloadFileClientInputModel>((param, model) => model.RemoteFile = param, 0)
							.PromtIfEmpty("Remote file")))
				.RegisterArgument(
					ArgumentCondition.Or(
						Argument.Create<DownloadFileClientInputModel>((param, model) => model.LocalFile = param, "-l", "-local"),
						Argument.Create<DownloadFileClientInputModel>((param, model) => model.LocalFile = param, 1)
							.PromtIfEmpty("Local file"))).Execute;

			_commandDictionary[CommandNameMetadata.ChangeDirectory] = new ArgumentClientInput<ChangeDirectoryClientInputModel>(model => _clientFlowService.ChangeDirectory(model.Path))
				.RegisterArgument(
					ArgumentCondition.Or(
						Argument.Create<ChangeDirectoryClientInputModel>((param, model) => model.Path = param, "-p", "-path"),
						Argument.Create<ChangeDirectoryClientInputModel>((param, model) => model.Path = param, 0))).Execute;

			_commandDictionary[CommandNameMetadata.ListDirectory] = new ArgumentClientInput<ListDirectoryClientInputModel>(model => _clientFlowService.ListDirectory(model.Path))
				.RegisterArgument(
					ArgumentCondition.Or(
						Argument.Create<ListDirectoryClientInputModel>((param, model) => model.Path = param, "-p", "-path"),
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
							.PromtIfEmpty("Remote port"))).Execute;

			_commandDictionary[CommandNameMetadata.Upload] = new ArgumentClientInput<UploadFileClientInputModel>(model => _clientFlowService.UploadFile(model.LocalFile, model.RemoteFile))
				.RegisterArgument(
					ArgumentCondition.Or(
						Argument.Create<UploadFileClientInputModel>((param, model) => model.LocalFile = param, "-l", "-local"),
						Argument.Create<UploadFileClientInputModel>((param, model) => model.LocalFile = param, 0)
							.PromtIfEmpty("Local file")))
				.RegisterArgument(
					ArgumentCondition.Or(
						Argument.Create<UploadFileClientInputModel>((param, model) => model.RemoteFile = param, "-r", "-remote"),
						Argument.Create<UploadFileClientInputModel>((param, model) => model.LocalFile = param, 1)
							.PromtIfEmpty("Remote file"))).Execute;

			_commandDictionary[CommandNameMetadata.Authenticate] = AuthenticateHandler;
			_commandDictionary[CommandNameMetadata.User] = UserHandler;
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
				password = ConsoleHelper.ReadSecureString();
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
	}
}

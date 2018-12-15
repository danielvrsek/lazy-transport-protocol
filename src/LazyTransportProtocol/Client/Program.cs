using LazyTransportProtocol.Client.Exceptions;
using LazyTransportProtocol.Client.Services;
using LazyTransportProtocol.Core.Application.IO;
using LazyTransportProtocol.Core.Domain.Exceptions;
using LazyTransportProtocol.Core.Domain.Exceptions.Response;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Client
{
	internal class Program
	{
		private static ClientInputService clientInputService = new ClientInputService();

		private static void Main(string[] args)
		{
			Console.WriteLine("LazyTransportProtocol client v0.9");
			Console.WriteLine();

			List<string> commands = new List<string>();

			foreach (string c in args)
			{
				if (c.StartsWith('/'))
				{
					commands.Add(c.Substring(1, c.Length - 1));
				}
				else if (commands.Any())
				{
					commands[commands.Count - 1] = commands[commands.Count - 1] + " " + c;
				}
			}

			foreach (var command in commands)
			{
				ExecuteCommand(command);
			}

			string commandRequest;

			do
			{
				if (clientInputService.Username != null && clientInputService.Host != null)
				{
					Console.Write($"{clientInputService.Username}@{clientInputService.Host}:{clientInputService.CurrentFolder}");
				}

				Console.Write('>');
				commandRequest = Console.ReadLine();
			}
			while (ExecuteCommand(commandRequest));
		}

		private static bool ExecuteCommand(string commandRequest)
		{
			try
			{
				return clientInputService.Execute(commandRequest);
			}
			catch (CommandException commandException)
			{
				Console.WriteLine(commandException.Message);
			}
			catch (InvalidResponseException)
			{
				Console.WriteLine("Invalid response from server.");
			}
			catch (CustomException e)
			{
				Console.WriteLine("Error occured: " + e.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.ReadLine();
			}

			return false;
		}
	}
}
using LazyTransportProtocol.Client.Exceptions;
using LazyTransportProtocol.Client.Services;
using LazyTransportProtocol.Core.Domain.Exceptions;
using LazyTransportProtocol.Core.Domain.Exceptions.Response;
using System;

namespace LazyTransportProtocol.Client
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			ClientInputService clientInputService = new ClientInputService();

			Console.WriteLine("LazyTransportProtocol client v0.9");
			Console.WriteLine();

			string commandRequest;

			while (true)
			{
				Console.Write('>');
				commandRequest = Console.ReadLine();

				if (commandRequest == "exit")
				{
					break;
				}

				try
				{
					clientInputService.Execute(commandRequest);
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
					break;
				}
			}
		}
	}
}
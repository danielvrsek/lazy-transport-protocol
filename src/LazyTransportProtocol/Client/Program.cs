using LazyTransportProtocol.Client.Exceptions;
using LazyTransportProtocol.Client.Services;
using LazyTransportProtocol.Core.Application.Protocol;
using LazyTransportProtocol.Core.Application.Protocol.Flow;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Transport;
using LazyTransportProtocol.Core.Application.Transport.Requests;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace LazyTransportProtocol.Client
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			ClientInputService clientInputService = new ClientInputService();

			Console.WriteLine("LazyTransportProtocol client v0.1");
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
				catch
				{
					Console.WriteLine("Unexpected error occured. Exiting...");
					Console.ReadLine();
					break;
				}
			}
		}
	}
}
using LazyTransportProtocol.Core.Application.Protocol;
using LazyTransportProtocol.Core.Application.Protocol.Flow;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Transport;
using LazyTransportProtocol.Core.Application.Transport.Requests;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LazyTransportProtocol.Client
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			ProtocolConnectionFlowService flowService = new ProtocolConnectionFlowService();

			Console.WriteLine("Connecting...");
			flowService.Connect();

			Console.WriteLine("Authenticating...");
			if (flowService.Authenticate("vrsek", "1234"))
			{
				Console.WriteLine("Authentication successful.");
			}
			else
			{
				Console.WriteLine("Authentication unsuccessful.");
				Console.WriteLine("Creating new user...");

				flowService.CreateNewUser("vrsek", "1234");
			}

			flowService.ListDirectory("/");

			Console.WriteLine("Disconnecting...");
			flowService.Disconnect();
		}
	}
}
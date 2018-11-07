using LazyTransportProtocol.Core.Application.Protocol;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Transport;
using LazyTransportProtocol.Core.Application.Transport.Requests;
using System;

namespace LazyTransportProtocol.Client
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			//var x = new ProtocolRequestExecutor().Execute(new ListDirectoryClientRequest());
			var y = new TransportRequestExecutor().Execute(new ConnectToServerRequest());

			Console.WriteLine("");
		}
	}
}
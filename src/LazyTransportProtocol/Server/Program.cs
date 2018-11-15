using LazyTransportProtocol.Core.Application.Protocol;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			ProtocolRequestListener listener = new ProtocolRequestListener();
			listener.Listen(1234);
		}
	}
}
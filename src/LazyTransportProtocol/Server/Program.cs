using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
	public class StateObject
	{
		// Client  socket.
		public Socket WorkSocket = null;

		// Size of receive buffer.
		public const int BufferSize = 1024;

		// Receive buffer.
		public byte[] Buffer = new byte[BufferSize];

		// Received data string.
		public StringBuilder Sb = new StringBuilder();
	}

	internal class Program
	{
		public static string data = null;

		private static void Main(string[] args)
		{
			StartListening();
		}

		public static void StartListening()
		{
			byte[] bytes = new Byte[1024];

			// Establish the local endpoint for the socket.
			// Dns.GetHostName returns the name of the
			// host running the application.
			IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
			IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 1234);

			// Create a TCP/IP socket.
			Socket listener = new Socket(ipAddress.AddressFamily,
				SocketType.Stream, ProtocolType.Tcp);

			// Bind the socket to the local endpoint and
			// listen for incoming connections.
			try
			{
				listener.Bind(localEndPoint);
				listener.Listen(10);

				// Start listening for connections.
				while (true)
				{
					Console.WriteLine("Waiting for a connection...");
					// Program is suspended while waiting for an incoming connection.
					Socket handler = listener.Accept();
					data = null;

					// An incoming connection needs to be processed.
					while (true)
					{
						int bytesRec = handler.Receive(bytes);
						data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
						if (data.IndexOf(";") > -1)
						{
							break;
						}
					}

					// Show the data on the console.
					Console.WriteLine("Text received : {0}", data);

					// Echo the data back to the client.
					byte[] msg = Encoding.ASCII.GetBytes("OK");

					handler.Send(msg);
					handler.Shutdown(SocketShutdown.Both);
					handler.Close();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			Console.WriteLine("\nPress ENTER to continue...");
			Console.Read();
		}
	}
}
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
			Console.WriteLine("Hello World!");

			var x = new ProtocolConnectionFlowService();
			//StartClient();
			//var x = new ProtocolRequestExecutor().Execute(new ListDirectoryClientRequest());
			x.StartConnection();
			x.BeginHandshake();
			x.EndConnection();

			Console.WriteLine("");
		}

		public static void StartClient()
		{
			// Data buffer for incoming data.
			byte[] bytes = new byte[1024];

			// Connect to a remote device.
			try
			{
				// Establish the remote endpoint for the socket.
				// This example uses port 11000 on the local computer.
				IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
				IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
				IPEndPoint remoteEP = new IPEndPoint(ipAddress, 1234);

				// Create a TCP/IP  socket.
				Socket sender = new Socket(ipAddress.AddressFamily,
					SocketType.Stream, ProtocolType.Tcp);

				// Connect the socket to the remote endpoint. Catch any errors.
				try
				{
					sender.Connect(remoteEP);

					Console.WriteLine("Socket connected to {0}",
						sender.RemoteEndPoint.ToString());

					// Encode the data string into a byte array.
					byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

					// Send the data through the socket.
					int bytesSent = sender.Send(msg);

					// Receive the response from the remote device.
					int bytesRec = sender.Receive(bytes);
					Console.WriteLine("Echoed test = {0}",
						Encoding.ASCII.GetString(bytes, 0, bytesRec));

					// Release the socket.
					sender.Shutdown(SocketShutdown.Both);
					sender.Close();
				}
				catch (ArgumentNullException ane)
				{
					Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
				}
				catch (SocketException se)
				{
					Console.WriteLine("SocketException : {0}", se.ToString());
				}
				catch (Exception e)
				{
					Console.WriteLine("Unexpected exception : {0}", e.ToString());
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}
	}
}
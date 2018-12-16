using LazyTransportProtocol.Core.Application.Client.Protocol.Model.Abstractions;
using System.Net;

namespace LazyTransportProtocol.Core.Application.Client.Protocol.Model
{
	public class SocketConnectionParameters : IRemoteConnectionParameters
	{
		public IPAddress IPAddress { get; set; }

		public int Port { get; set; }
	}
}
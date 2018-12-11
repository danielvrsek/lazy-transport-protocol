﻿using LazyTransportProtocol.Core.Application.Transport.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions;
using System;
using System.Net.Sockets;

namespace LazyTransportProtocol.Core.Application.Transport.Infrastructure
{
	public class SocketServerConnection : IServerConnection
	{
		private TransportRequestExecutor _transportExecutor = new TransportRequestExecutor();

		public SocketServerConnection(Socket sender)
		{
			Sender = sender;
		}

		public Socket Sender { get; }

		public byte[] Send(byte[] data)
		{
			var response = _transportExecutor.Execute(new SendDataRequest
			{
				Data = data,
				Sender = Sender
			});

			return response.ResponseData;
		}

		public void SendAsync(byte[] data, Action<byte[]> responseCallback)
		{
			throw new NotImplementedException();
		}

		public void Disconnect()
		{
			var response = _transportExecutor.Execute(new EndConnectionRequest
			{
				Sender = Sender
			});
		}

		public bool IsAlive()
		{
			return Sender.Connected;
		}
	}
}
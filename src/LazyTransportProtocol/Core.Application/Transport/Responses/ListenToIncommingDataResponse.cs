using LazyTransportProtocol.Core.Application.Transport.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Transport.Responses
{
	public delegate void ClientConnected(IClientConnection connection);

	public delegate void DataReceived(IClientConnection connection, byte[] data);

	public delegate void ErrorOccured(ErrorContext ctx);

	public class ErrorContext
	{
		public Exception Exception { get; set; }

		public SocketError? SocketError { get; set; }
	}

	public class ListenToIncommingDataResponse : IResponse
	{
		public event ClientConnected ClientConnected;

		public event DataReceived DataReceived;

		public event ErrorOccured ErrorOccured;

		internal void OnClientConnected(IClientConnection connection)
		{
			ClientConnected(connection);
		}

		internal void OnDataReceived(IClientConnection connection, byte[] data)
		{
			DataReceived(connection, data);
		}

		internal void OnErrorOccured(ErrorContext ctx)
		{
			ErrorOccured(ctx);
		}
	}
}
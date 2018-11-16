using LazyTransportProtocol.Core.Application.Flow;
using LazyTransportProtocol.Core.Application.Infrastructure;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Configuration;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Application.Transport;
using LazyTransportProtocol.Core.Application.Transport.DataModel;
using LazyTransportProtocol.Core.Application.Transport.Requests;
using LazyTransportProtocol.Core.Application.Transport.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Common;
using LazyTransportProtocol.Core.Transport.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Flow
{
	public class ProtocolConnectionFlowService
	{
		private readonly IRemoteRequestExecutor remoteExecutor = new RemoteProtocolRequestExecutor();

		public void Connect()
		{
			remoteExecutor.Connect("127.0.0.1", 1234);
		}

		public bool CreateNewUser(string username, string password)
		{
			AcknowledgementResponse response = remoteExecutor.Execute(new CreateUserRequest
			{
				Username = username,
				Password = password
			});

			return response.IsSuccessful;
		}

		public bool Authenticate(string username, string password)
		{
			AcknowledgementResponse response = remoteExecutor.Execute(new AuthenticationRequest
			{
				Username = username,
				Password = password
			});

			return response.IsSuccessful;
		}

		public void ListDirectory(string path)
		{
			var response = remoteExecutor.Execute(new ListDirectoryClientRequest
			{
				Path = "/"
			});

			Console.WriteLine(String.Join(", ", response.RemoteDirectories));
		}

		public void Disconnect()
		{
			remoteExecutor.Disconnect();
		}
	}
}
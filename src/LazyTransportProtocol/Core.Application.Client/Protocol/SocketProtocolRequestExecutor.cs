using LazyTransportProtocol.Core.Application.Client.Protocol.Model;
using LazyTransportProtocol.Core.Application.Client.Protocol.Model.Abstractions;
using LazyTransportProtocol.Core.Application.Protocol.Model;
using LazyTransportProtocol.Core.Application.Protocol.Requests.Abstractions;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Responses.Abstractions;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Transport;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Exceptions;
using LazyTransportProtocol.Core.Domain.Exceptions.Client;
using LazyTransportProtocol.Core.Domain.Exceptions.Response;
using System;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Client.Protocol
{
	public class SocketProtocolRequestExecutor : IRemoteRequestExecutor
	{
		private IServerConnection _connection;

		private readonly ProtocolEncoder _encoder = new ProtocolEncoder();
		private readonly ITransport _transport = new TransportLayer();

		public void Connect(IRemoteConnectionParameters parameters)
		{
			Connect((SocketConnectionParameters)parameters);
		}

		public void Connect(SocketConnectionParameters parameters)
		{
			if (_connection != null)
			{
				_connection.Disconnect();
			}

			_connection = _transport.Connect(parameters.IPAddress, parameters.Port);
		}

		public TResponse Execute<TResponse>(IProtocolRequest<TResponse> request)
			where TResponse : class, IProtocolResponse, new()
		{
			if (_connection == null)
			{
				throw new ConnectionRequiredException();
			}

			MessageHeadersDictionary headers = new MessageHeadersDictionary();

			byte[] requestHeaders = ProtocolMessageHeaderSerializer.Serialize(headers);
			byte[] requestBody = ProtocolBodySerializer.Serialize(request);
			byte[] requestIdentifier = _encoder.Encode(request.GetIdentifier());
			ArraySegment<byte> serializedRequest = ProtocolSerializer.Serialize(requestIdentifier, requestHeaders, requestBody);

			byte[] responseEncoded = _connection.Send(serializedRequest);

			var deserializedObject = ProtocolSerializer.Deserialize(responseEncoded);
			string expectedIdentifier = new TResponse().GetIdentifier();
			string identifier = _encoder.Decode(deserializedObject.ControlCommand);

			if (identifier != expectedIdentifier)
			{
				if (identifier == ErrorResponse.Identifier)
				{
					ErrorResponse errorResponse = ProtocolBodySerializer.Deserialize<ErrorResponse>(deserializedObject.Body);
					throw new CustomException(errorResponse.Message);
				}

				throw new InvalidResponseException();
			}

			TResponse response = ProtocolBodySerializer.Deserialize<TResponse>(deserializedObject.Body);

			return response;
		}

		public Task<TResponse> ExecuteAsync<TResponse>(IProtocolRequest<TResponse> request)
			where TResponse : class, IProtocolResponse, new()
		{
			throw new NotImplementedException();
		}

		public void Disconnect()
		{
			_connection.Disconnect();
		}
	}
}
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

			string requestHeaders = ProtocolMessageHeaderSerializer.Serialize(headers);
			string requestBody = ProtocolBodySerializer.Serialize(request);
			string requestIdentifier = request.GetIdentifier();
			string requestString = ProtocolSerializer.Serialize(requestIdentifier, requestHeaders, requestBody);
			byte[] requestEncoded = _encoder.Encode(requestString);

			byte[] responseEncoded = _connection.Send(requestEncoded);
			string responseDecoded = _encoder.Decode(responseEncoded);

			var requestObject = ProtocolSerializer.Deserialize(responseDecoded);
			string identifier = new TResponse().GetIdentifier();

			if (requestObject.ControlCommand != identifier)
			{
				if (requestObject.ControlCommand == ErrorResponse.Identifier)
				{
					ErrorResponse errorResponse = ProtocolBodySerializer.Deserialize<ErrorResponse>(requestObject.Body);
					throw new CustomException(errorResponse.Message);
				}

				throw new InvalidResponseException();
			}

			TResponse response = ProtocolBodySerializer.Deserialize<TResponse>(requestObject.Body);

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
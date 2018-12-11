using LazyTransportProtocol.Core.Application.Infrastructure;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Model;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.Metadata;
using LazyTransportProtocol.Core.Application.Protocol.Model;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Application.Transport;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Common;
using LazyTransportProtocol.Core.Domain.Exceptions;
using LazyTransportProtocol.Core.Domain.Exceptions.Client;
using LazyTransportProtocol.Core.Domain.Exceptions.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Protocol
{
	public class SocketProtocolRequestExecutor : IRemoteRequestExecutor
	{
		private IServerConnection _connection;

		private readonly IEncoder _encoder = new ProtocolEncoder();
		private readonly IDecoder _decoder = new ProtocolDecoder();
		private readonly ITransport _transport = new TransportLayer();

		private const string _separator = ";";
		private readonly ProtocolVersion _protocolVersion = ProtocolVersion.V1_0;
		private const int _maxRequestLength = 2048;

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
			string responseDecoded = _decoder.Decode(responseEncoded);

			var requestObject = ProtocolDeserializer.Deserialize(responseDecoded);
			string identifier = new TResponse().GetIdentifier();

			if (requestObject.ControlCommand != identifier)
			{
				if (requestObject.ControlCommand == ErrorResponse.Identifier)
				{
					ErrorResponse errorResponse = ProtocolBodyDeserializer.Deserialize<ErrorResponse>(requestObject.Body);
					throw new CustomException(errorResponse.Message);
				}

				throw new InvalidResponseException();
			}

			TResponse response = ProtocolBodyDeserializer.Deserialize<TResponse>(requestObject.Body);

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
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
using LazyTransportProtocol.Core.Domain.Exceptions.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Protocol
{
	public class SocketProtocolRequestExecutor : IRemoteRequestExecutor
	{
		private IConnection _connection;

		private readonly IEncoder _encoder = new ProtocolEncoder();
		private readonly IDecoder _decoder = new ProtocolDecoder();
		private readonly ITransport _transport = new TransportLayer();

		private const string _separator = ";";
		private readonly ProtocolVersion _protocolVersion = ProtocolVersion.V1_0;
		private const int _maxRequestLength = 16384;

		public void Connect(IRemoteConnectionParameters parameters)
		{
			Connect((SocketConnectionParameters)parameters);
		}

		public void Connect(SocketConnectionParameters parameters)
		{
			_connection = _transport.Connect(parameters.IPAddress, parameters.Port);
			_connection.State = new ProtocolState
			{
				// handshake
				AgreedHeaders = new AgreedHeadersDictionary(";", 1024, ProtocolVersion.Handshake)
			};

			AcknowledgementResponse response = Execute(new HandshakeRequest
			{
				ProtocolVersion = _protocolVersion,
				BufferSize = _maxRequestLength,
				Separator = _separator
			});

			ProtocolState state = (ProtocolState)_connection.State;

			state.AgreedHeaders = new AgreedHeadersDictionary(_separator, _maxRequestLength, _protocolVersion);
		}

		public TResponse Execute<TResponse>(IProtocolRequest<TResponse> request)
			where TResponse : class, IProtocolResponse, new()
		{
			if (_connection == null)
			{
				throw new ConnectionRequiredException();
			}

			ProtocolState state = (ProtocolState)_connection.State;

			MessageHeadersDictionary headers = new MessageHeadersDictionary();

			string requestHeaders = ProtocolMessageHeaderSerializer.Serialize(headers, state.AgreedHeaders.ProtocolVersion);
			string requestBody = ProtocolBodySerializer.Serialize(request, state.AgreedHeaders.ProtocolVersion);
			string requestIdentifier = request.GetIdentifier(state.AgreedHeaders.ProtocolVersion);
			string requestString = ProtocolSerializer.Serialize(requestIdentifier, requestHeaders, requestBody, state.AgreedHeaders);
			byte[] requestEncoded = _encoder.Encode(requestString + "<EOF>");

			byte[] responseEncoded = _connection.Send(requestEncoded);
			string responseDecoded = _decoder.Decode(responseEncoded);

			var requestObject = ProtocolDeserializer.Deserialize(responseDecoded, state.AgreedHeaders, state.AgreedHeaders.ProtocolVersion);
			TResponse response = ProtocolBodyDeserializer.Deserialize<TResponse>(requestObject.Body, state.AgreedHeaders.ProtocolVersion);

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
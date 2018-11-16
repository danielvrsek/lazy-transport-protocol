using LazyTransportProtocol.Core.Application.Infrastructure;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.Metadata;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Application.Transport;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Protocol
{
	public class RemoteProtocolRequestExecutor : IRemoteRequestExecutor
	{
		private IConnection _connection;

		private readonly IEncoder _encoder = new ProtocolEncoder();
		private readonly IDecoder _decoder = new ProtocolDecoder();
		private readonly ITransport _transport = new TransportLayer();

		private const string _separator = ";";
		private readonly ProtocolVersion _protocolVersion = ProtocolVersion.V1_0;

		public void Connect(string ipAdress, int port)
		{
			_connection = _transport.Connect(ipAdress, port);
			_connection.State = new ProtocolState
			{
				AgreedHeaders = new AgreedHeadersDictionary(";", ProtocolVersion.Handshake)
			};

			AcknowledgementResponse response = Execute(new HandshakeRequest
			{
				ProtocolVersion = _protocolVersion,
				Separator = _separator
			});

			ProtocolState state = (ProtocolState)_connection.State;

			state.AgreedHeaders = new AgreedHeadersDictionary(_separator, _protocolVersion);
		}

		public TResponse Execute<TResponse>(IProtocolRequest<TResponse> request)
			where TResponse : class, IProtocolResponse, new()
		{
			ProtocolState state = (ProtocolState)_connection.State;

			string requestBody = ProtocolBodySerializer.Serialize(request, state.AgreedHeaders.ProtocolVersion);
			string requestString = SerializeHelper.SerializeRequestString(request.GetIdentifier(state.AgreedHeaders.ProtocolVersion), requestBody, state.AgreedHeaders);

			string requestDecoded = requestString + "<EOF>";
			byte[] requestEncoded = _encoder.Encode(requestDecoded);
			byte[] responseEncoded = _connection.Send(requestEncoded);
			string responseDecoded = _decoder.Decode(responseEncoded);

			var requestObject = DeserializeHelper.DeserializeRequestString(responseDecoded, state.AgreedHeaders, state.AgreedHeaders.ProtocolVersion);
			TResponse response = ProtocolBodyDeserializer.Deserialize<TResponse>(requestObject.Body, state.AgreedHeaders.ProtocolVersion);

			return response;
		}

		public Task<TResponse> ExecuteAsync<TResponse>(IProtocolRequest<TResponse> request)
			where TResponse : class, IProtocolResponse, new()
		{
			throw new System.NotImplementedException();
		}

		public void Disconnect()
		{
			_connection.Disconnect();
		}
	}
}
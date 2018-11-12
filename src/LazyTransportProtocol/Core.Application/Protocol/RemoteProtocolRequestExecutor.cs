using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Application.Transport;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Common;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Protocol
{
	public class RemoteProtocolRequestExecutor : IRemoteRequestExecutor
	{
		private IConnection _connection;

		private readonly IEncoder _encoder = new ProtocolEncoder();
		private readonly IDecoder _decoder = new ProtocolDecoder();
		private readonly ITransport _transport = new TransportLayer();
		private readonly ProtocolVersion _protocolVersion;

		public RemoteProtocolRequestExecutor(ProtocolVersion protocolVersion)
		{
			_protocolVersion = protocolVersion;
		}

		public void Connect(string ipAdress, int port)
		{
			_connection = _transport.Connect(ipAdress, port);
		}

		public TResponse Execute<TResponse>(IProtocolRequest<TResponse> request)
			where TResponse : class, IProtocolResponse, new()
		{
			Contract.Requires(_connection != null, "Connection was not established.");

			string requestDecoded = request.Serialize(_protocolVersion);
			byte[] requestEncoded = _encoder.Encode(requestDecoded);
			byte[] responseEncoded = _connection.Send(requestEncoded);
			string responseDecoded = _decoder.Decode(responseEncoded);

			TResponse response = new TResponse();
			response.Deserialize(responseDecoded, _protocolVersion);

			return response;
		}

		public Task<TResponse> ExecuteAsync<TResponse>(IProtocolRequest<TResponse> request)
			where TResponse : class, IProtocolResponse, new()
		{
			throw new System.NotImplementedException();
		}
	}
}
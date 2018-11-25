using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Protocol.Handlers
{
	public class HandshakeRequestHandler : IProtocolRequestHandler<HandshakeRequest, AcknowledgementResponse>
	{
		public AcknowledgementResponse GetResponse(HandshakeRequest request)
		{
			Console.WriteLine($"Handshake. Data: {request.ProtocolVersion}, {request.Separator}");

			return new AcknowledgementResponse
			{
				Code = 200
			};
		}

		public Task<AcknowledgementResponse> GetResponseAsync(HandshakeRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
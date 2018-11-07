using LazyTransportProtocol.Core.Application.Transport.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Transport.Requests;
using LazyTransportProtocol.Core.Application.Transport.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Transport.Handlers
{
	public class ConnectToServerRequestHandler : ITransportRequestHandler<ConnectToServerRequest, ConnectToServerResponse>
	{
		public ConnectToServerResponse GetResponse(ConnectToServerRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<ConnectToServerResponse> GetResponseAsync(ConnectToServerRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
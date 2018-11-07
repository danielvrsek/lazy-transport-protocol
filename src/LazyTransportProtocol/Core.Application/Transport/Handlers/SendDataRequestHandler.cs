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
	public class SendDataRequestHandler : ITransportRequestHandler<SendDataRequest, SendDataResponse>
	{
		public SendDataResponse GetResponse(SendDataRequest request)
		{
			return new SendDataResponse();
		}

		public Task<SendDataResponse> GetResponseAsync(SendDataRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
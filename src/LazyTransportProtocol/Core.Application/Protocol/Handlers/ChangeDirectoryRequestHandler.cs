using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
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
	public class ChangeDirectoryRequestHandler : IProtocolRequestHandler<ChangeDirectoryClientRequest, ChangeDirectoryResponse>
	{
		public ChangeDirectoryResponse GetResponse(ChangeDirectoryClientRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<ChangeDirectoryResponse> GetResponseAsync(ChangeDirectoryClientRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
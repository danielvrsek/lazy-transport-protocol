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
	public class ListDirectoryRequestHandler : IProtocolRequestHandler<ListDirectoryClientRequest, ListDirectoryResponse>
	{
		public ListDirectoryResponse GetResponse(ListDirectoryClientRequest request)
		{
			return new ListDirectoryResponse();
		}

		public Task<ListDirectoryResponse> GetResponseAsync(ListDirectoryClientRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
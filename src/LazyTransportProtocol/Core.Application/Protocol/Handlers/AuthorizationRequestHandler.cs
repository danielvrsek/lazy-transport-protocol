using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Protocol.Handlers
{
	public class AuthorizationRequestHandler : IRequestHandler<AuthorizationRequest, AuthorizationResponse>
	{
		public AuthorizationResponse GetResponse(AuthorizationRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<AuthorizationResponse> GetResponseAsync(AuthorizationRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
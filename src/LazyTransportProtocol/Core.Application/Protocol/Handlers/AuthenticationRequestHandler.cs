using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Services;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Protocol.Handlers
{
	public class AuthenticationRequestHandler : IRequestHandler<AuthenticationRequest, AcknowledgementResponse>
	{
		public AcknowledgementResponse GetResponse(AuthenticationRequest request)
		{
			IAuthenticationService authenticationService = new AuthenticationService();

			bool isSuccessful = authenticationService.Authenticate(request.Username, request.Password);

			return new AcknowledgementResponse
			{
				IsSuccessful = isSuccessful
			};
		}

		public Task<AcknowledgementResponse> GetResponseAsync(AuthenticationRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
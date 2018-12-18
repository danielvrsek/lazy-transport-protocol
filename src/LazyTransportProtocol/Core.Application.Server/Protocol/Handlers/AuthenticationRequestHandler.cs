using JWT;
using JWT.Algorithms;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Server.Configuration;
using LazyTransportProtocol.Core.Application.Server.Protocol.Handlers.Abstraction;
using LazyTransportProtocol.Core.Application.Server.Protocol.Metadata;
using LazyTransportProtocol.Core.Application.Server.Protocol.Model;
using LazyTransportProtocol.Core.Application.Server.Services;
using LazyTransportProtocol.Core.Application.Server.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Server.Protocol.Handlers
{
	public class AuthenticationRequestHandler : IProtocolRequestHandler<AuthenticationRequest, AuthenticationResponse>
	{
		public AuthenticationResponse GetResponse(AuthenticationRequest request)
		{
			IAuthenticationService authenticationService = new AuthenticationService();

			bool isSuccessful = authenticationService.Authenticate(request.Username, request.Password);

			if (isSuccessful)
			{
				List<Claim> claims = new List<Claim>
				{
					new Claim(ClaimTypesMetadata.Username, request.Username)
				};

				JWTPayload payload = new JWTPayload
				{
					Claims = claims
				};

				string secret = ServerConfiguration.Instance().ServerSecret;

				IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
				IJsonSerializer serializer = new FastJSONSerializer();
				IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
				IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

				string token = encoder.Encode(payload, secret);

				return new AuthenticationResponse
				{
					Code = 200,
					AuthenticationToken = token
				};
			}

			return new AuthenticationResponse
			{
				Code = 400
			};
		}

		public Task<AuthenticationResponse> GetResponseAsync(AuthenticationRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Services;
using LazyTransportProtocol.Core.Application.Protocol.Configuration;
using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.Metadata;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Security;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Protocol.Handlers
{
	public class AuthenticationRequestHandler : IRequestHandler<AuthenticationRequest, AuthenticationResponse>
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

				string secret = ServerConfiguration.Instance().ServerSecret;

				IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
				IJsonSerializer serializer = new JsonNetSerializer();
				IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
				IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

				string token = encoder.Encode(claims, secret);

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
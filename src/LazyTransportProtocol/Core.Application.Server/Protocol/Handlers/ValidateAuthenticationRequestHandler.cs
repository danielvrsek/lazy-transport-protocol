using JWT;
using LazyTransportProtocol.Core.Application.Server.Configuration;
using LazyTransportProtocol.Core.Application.Server.Protocol.Handlers.Abstraction;
using LazyTransportProtocol.Core.Application.Server.Protocol.Model;
using LazyTransportProtocol.Core.Application.Server.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Server.Protocol.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;
using fastJSON;
using LazyTransportProtocol.Core.Application.Server.Services;

namespace LazyTransportProtocol.Core.Application.Server.Protocol.Handlers
{
	internal class ValidateAuthenticationRequestHandler : IProtocolRequestHandler<ValidateAuthenticationRequest, ValidateAuthenticationResponse>
	{
		public ValidateAuthenticationResponse GetResponse(ValidateAuthenticationRequest request)
		{
			try
			{
				string secret = ServerConfiguration.Instance().ServerSecret;

				IJsonSerializer serializer = new FastJSONSerializer();
				IDateTimeProvider provider = new UtcDateTimeProvider();
				IJwtValidator validator = new JwtValidator(serializer, provider);
				IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
				IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

				var json = decoder.Decode(request.AuthenticationToken, secret, true);

				JWTPayload payload = JSON.ToObject<JWTPayload>(json);

				return new ValidateAuthenticationResponse
				{
					Code = 200,
					Claims = payload.Claims
				};
			}
			catch (Exception)
			{
				return new ValidateAuthenticationResponse
				{
					Code = 403
				};
			}
		}

		public Task<ValidateAuthenticationResponse> GetResponseAsync(ValidateAuthenticationRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
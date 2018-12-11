using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Configuration;
using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Security;
using LazyTransportProtocol.Core.Domain.Exceptions.Authorization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Protocol.Handlers
{
	internal class ValidateAuthenticationRequestHandler : IProtocolRequestHandler<ValidateAuthenticationRequest, ValidateAuthenticationResponse>
	{
		public ValidateAuthenticationResponse GetResponse(ValidateAuthenticationRequest request)
		{
			try
			{
				string secret = ServerConfiguration.Instance().ServerSecret;

				IJsonSerializer serializer = new JsonNetSerializer();
				IDateTimeProvider provider = new UtcDateTimeProvider();
				IJwtValidator validator = new JwtValidator(serializer, provider);
				IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
				IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

				var json = decoder.Decode(request.AuthenticationToken, secret, true);

				List<Claim> claims = JsonConvert.DeserializeObject<List<Claim>>(json);

				return new ValidateAuthenticationResponse
				{
					Code = 200,
					Claims = claims
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
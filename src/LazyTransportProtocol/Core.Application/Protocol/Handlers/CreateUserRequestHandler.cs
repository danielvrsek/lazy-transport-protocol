using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Services;
using LazyTransportProtocol.Core.Application.Protocol.Model;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Handlers
{
	public class CreateUserRequestHandler : IProtocolRequestHandler<CreateUserRequest, AcknowledgementResponse>
	{
		public AcknowledgementResponse GetResponse(CreateUserRequest request)
		{
			IUserService userService = new UserService();
			IPasswordEncryptor passwordEncryptor = new Sha1PasswordEncryptor();
			string encryptedPassword = passwordEncryptor.Encrypt(request.Password);

			bool isSuccessful = true;

			try
			{
				userService.CreateNew(new UserSecret
				{
					Username = request.Username,
					Password = encryptedPassword
				});
			}
			catch
			{
				isSuccessful = false;
			}

			return new AcknowledgementResponse
			{
				IsSuccessful = isSuccessful
			};
		}
	}
}
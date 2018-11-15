using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Services;
using LazyTransportProtocol.Core.Application.Protocol.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class AuthenticationService : IAuthenticationService
	{
		public bool Authenticate(string username, string password)
		{
			string encryptedPassword = new Sha1PasswordEncryptor().Encrypt(password);

			IUserSecretAccessor userSecretAccessor = new UserSecretAccessor();
			UserSecret secretInfo = userSecretAccessor.GetSecretForUsername(username);

			return secretInfo?.Password == encryptedPassword ? true : false;
		}
	}
}
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Services;
using LazyTransportProtocol.Core.Application.Protocol.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class UserService : IUserService
	{
		public UserSecret CreateNew(string username, string password)
		{
			IUserSecretAccessor secretAccessor = new UserSecretAccessor();

			IPasswordEncryptor passwordEncryptor = new Sha1PasswordEncryptor();
			string encryptedPassword = passwordEncryptor.Encrypt(password);

			return secretAccessor.InsertNewSercret(new UserSecret
			{
				Username = username,
				Password = encryptedPassword
			});
		}

		public UserSecret Modify(string username, string password)
		{
			IUserSecretAccessor secretAccessor = new UserSecretAccessor();

			IPasswordEncryptor passwordEncryptor = new Sha1PasswordEncryptor();
			string encryptedPassword = passwordEncryptor.Encrypt(password);

			return secretAccessor.ModifySecret(new UserSecret
			{
				Username = username,
				Password = encryptedPassword
			});
		}

		public void Delete(string username)
		{
			IUserSecretAccessor secretAccessor = new UserSecretAccessor();

			secretAccessor.DeleteSecret(username);
		}

		public bool Exists(string username, string password)
		{
			IUserSecretAccessor secretAccessor = new UserSecretAccessor();

			IPasswordEncryptor passwordEncryptor = new Sha1PasswordEncryptor();
			string encryptedPassword = passwordEncryptor.Encrypt(password);

			UserSecret userSecret = secretAccessor.GetSecretForUsername(username);

			return encryptedPassword == userSecret?.Password;
		}
	}
}
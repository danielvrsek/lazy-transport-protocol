using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Services;
using LazyTransportProtocol.Core.Application.Protocol.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class UserService : IUserService
	{
		public UserSecret CreateNew(UserSecret userSecret)
		{
			IUserSecretAccessor secretAccessor = new UserSecretAccessor();

			return secretAccessor.InsertNewSercret(userSecret);
		}

		public UserSecret Modify(UserSecret userSecret)
		{
			IUserSecretAccessor secretAccessor = new UserSecretAccessor();

			secretAccessor.ModifySecret(userSecret);

			return userSecret;
		}

		public void Delete(string username)
		{
			IUserSecretAccessor secretAccessor = new UserSecretAccessor();

			secretAccessor.DeleteSecret(username);
		}

		public bool Exists(string username)
		{
			throw new NotImplementedException();
		}
	}
}
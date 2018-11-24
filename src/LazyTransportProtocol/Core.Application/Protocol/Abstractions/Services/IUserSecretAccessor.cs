using LazyTransportProtocol.Core.Application.Protocol.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Abstractions.Services
{
	public interface IUserSecretAccessor
	{
		UserSecret GetSecretForUsername(string username);

		UserSecret InsertNewSercret(UserSecret secretInfo);

		UserSecret ModifySecret(UserSecret userSecret);

		void DeleteSecret(string username);
	}
}
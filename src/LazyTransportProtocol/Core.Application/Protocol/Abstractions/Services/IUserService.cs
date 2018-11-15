using LazyTransportProtocol.Core.Application.Protocol.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Abstractions.Services
{
	public interface IUserService
	{
		bool Exists(string username);

		UserSecret CreateNew(UserSecret userSecret);

		UserSecret Modify(UserSecret userSecret);

		void Delete(string username);
	}
}
using LazyTransportProtocol.Core.Application.Protocol.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Abstractions.Services
{
	public interface IUserService
	{
		bool Exists(string username, string password);

		UserSecret CreateNew(string username, string password);

		UserSecret Modify(string username, string password);

		void Delete(string username);
	}
}
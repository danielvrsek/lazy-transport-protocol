using LazyTransportProtocol.Core.Application.Server.Protocol.Model;

namespace LazyTransportProtocol.Core.Application.Server.Services.Abstractions
{
	public interface IUserService
	{
		bool Exists(string username, string password);

		UserSecret CreateNew(string username, string password);

		UserSecret Modify(string username, string password);

		void Delete(string username);
	}
}
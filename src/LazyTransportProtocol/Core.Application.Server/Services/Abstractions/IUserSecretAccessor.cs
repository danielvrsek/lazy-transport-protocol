using LazyTransportProtocol.Core.Application.Server.Protocol.Model;

namespace LazyTransportProtocol.Core.Application.Server.Services.Abstractions
{
	public interface IUserSecretAccessor
	{
		UserSecret GetSecretForUsername(string username);

		UserSecret InsertNewSercret(UserSecret secretInfo);

		UserSecret ModifySecret(UserSecret userSecret);

		void DeleteSecret(string username);
	}
}
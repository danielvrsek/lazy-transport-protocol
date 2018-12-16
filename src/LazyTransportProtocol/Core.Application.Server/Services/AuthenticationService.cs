using LazyTransportProtocol.Core.Application.Server.Services.Abstractions;

namespace LazyTransportProtocol.Core.Application.Server.Services
{
	internal class AuthenticationService : IAuthenticationService
	{
		public bool Authenticate(string username, string password)
		{
			IUserService userService = new UserService();

			return userService.Exists(username, password);
		}
	}
}
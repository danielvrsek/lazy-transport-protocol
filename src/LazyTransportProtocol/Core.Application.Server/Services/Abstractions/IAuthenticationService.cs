namespace LazyTransportProtocol.Core.Application.Server.Services.Abstractions
{
	/// <summary>
	/// Abstraction for authentication service.
	/// </summary>
	public interface IAuthenticationService
	{
		/// <summary>
		/// Method for user authentication
		/// </summary>
		/// <param name="username">Username used for authentication</param>
		/// <param name="password">Unencrypted password</param>
		/// <returns>True if the combination of username and password is correct</returns>
		bool Authenticate(string username, string password);
	}
}
namespace LazyTransportProtocol.Core.Application.Protocol.Abstractions.Services
{
	public interface IAuthorizationService
	{
		bool CheckAccess(string username, string resource, string action);
	}
}
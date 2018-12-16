namespace LazyTransportProtocol.Core.Application.Server.Protocol.Security.Authorization
{
	internal class AuthorizationAction
	{
		public string Name { get; }

		public AuthorizationAction(string name)
		{
			Name = name;
		}
	}
}
namespace LazyTransportProtocol.Core.Application.Server.Protocol.Security.Authorization
{
	internal class AuthorizationResource
	{
		public string Name { get; }

		public AuthorizationResource(string name)
		{
			Name = name;
		}
	}
}
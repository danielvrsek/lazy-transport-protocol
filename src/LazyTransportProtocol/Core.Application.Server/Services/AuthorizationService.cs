using LazyTransportProtocol.Core.Application.Server.Configuration;
using LazyTransportProtocol.Core.Application.Server.Protocol.Security.Authentication.Abstraction;

namespace LazyTransportProtocol.Core.Application.Server.Services
{
	internal class AuthorizationService
	{
		private readonly ServerConfiguration _serverConfiguration;

		public AuthorizationService()
		{
			_serverConfiguration = ServerConfiguration.Instance();
		}

		public bool HasAccessToDirectory(IAuthenticationContext context, string path)
		{
			if (!IsChildOfRootDirectory(path))
			{
				return false;
			}

			return true;
		}

		private bool IsChildOfRootDirectory(string path)
		{
			string rootPath = IOService.GetAbsoluteRootFolder();
			path = IOService.TransformPath(path);

			return path.StartsWith(rootPath);
		}
	}
}
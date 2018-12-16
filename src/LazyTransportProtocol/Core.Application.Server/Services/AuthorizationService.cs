using LazyTransportProtocol.Core.Application.Server.Configuration;
using LazyTransportProtocol.Core.Application.Server.Protocol.Model.Abstraction.LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Services;

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
			string systemPath = IOService.TransformPath(path);

			string rel = PathExt.GetRelativePath(_serverConfiguration.RootFolder, systemPath);

			if (rel.StartsWith(".."))
			{
				return false;
			}

			return true;
		}
	}
}
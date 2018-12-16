using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Infrastructure;
using LazyTransportProtocol.Core.Application.Protocol.Configuration;
using LazyTransportProtocol.Core.Application.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
    public class AuthorizationService
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
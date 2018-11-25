using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Infrastructure;
using LazyTransportProtocol.Core.Application.Protocol.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
    public class AuthorizationService
    {
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

			string rel = Path.GetRelativePath(ServerConfiguration.RootFolder, systemPath);

			if (rel.StartsWith(".."))
			{
				return false;
			}

			return true;
		}
    }
}

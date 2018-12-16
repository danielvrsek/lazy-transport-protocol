using System;
using System.IO;

namespace LazyTransportProtocol.Core.Application.Services
{
	public static class PathExt
	{
		public static string GetRelativePath(string relativeTo, string path)
		{
			var uri = new Uri(relativeTo);
			var rel = Uri.UnescapeDataString(uri.MakeRelativeUri(new Uri(path)).ToString()).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
			if (rel.Contains(Path.DirectorySeparatorChar.ToString()) == false)
			{
				rel = $".{ Path.DirectorySeparatorChar }{ rel }";
			}
			return rel;
		}
	}
}
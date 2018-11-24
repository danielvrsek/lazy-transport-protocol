using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Client.Metadata
{
	public static class CommandNameMetadata
	{
		public const string List = "cd";

		public const string Authenticate = "login";

		public const string User = "user";

		public const string Connect = "connect";

		public const string Upload = "upload";

		public const string Download = "download";
	}
}

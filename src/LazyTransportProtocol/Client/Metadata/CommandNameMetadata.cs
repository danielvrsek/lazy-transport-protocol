using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Client.Metadata
{
	public static class CommandNameMetadata
	{
		public const string ListDirectory = "ls";

		public const string ChangeDirectory = "cd";

		public const string CreateDirectory = "mkdir";

		public const string Authenticate = "login";

		public const string User = "user";

		public const string Connect = "connect";

		public const string Upload = "upload";

		public const string Download = "download";

		public const string Exit = "exit";
	}
}

using LazyTransportProtocol.Core.Application.Protocol.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Requests
{
	public class DownloadFileRequest : AuthenticatedRequest<DownloadFileResponse>
	{
		public const string Identifier = "DOWNFILE";

		public string Filepath { get; set; }

		public int Offset { get; set; }

		public int Count { get; set; }

		public override string GetIdentifier()
		{
			return Identifier;
		}
	}
}
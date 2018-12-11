using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Infrastructure;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Requests
{
	public class DownloadFileRequest : IProtocolRequest<DownloadFileResponse>
	{
		public const string Identifier = "DOWNFILE";

		public string Filepath { get; set; }

		public int Offset { get; set; }

		public int Count { get; set; }

		public IAuthenticationContext AuthenticationContext { get; set; }

		public string GetIdentifier()
		{
			return Identifier;
		}
	}
}
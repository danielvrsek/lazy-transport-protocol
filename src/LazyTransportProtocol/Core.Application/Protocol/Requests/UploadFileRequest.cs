using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Infrastructure;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Requests
{
	public class UploadFileRequest : IProtocolRequest<AcknowledgementResponse>
	{
		public const string Identifier = "UPLOADFILE";

		public string Path { get; set; }

		public byte[] Data { get; set; }

		public int Offset { get; set; }

		public IAuthenticationContext AuthenticationContext { get; set; }

		public string GetIdentifier()
		{
			return Identifier;
		}
	}
}

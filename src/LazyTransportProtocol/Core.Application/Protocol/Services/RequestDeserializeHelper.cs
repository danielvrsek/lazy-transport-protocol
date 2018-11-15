using LazyTransportProtocol.Core.Application.Protocol.Metadata;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public static class RequestDeserializeHelper
	{
		public static MediumDeserializedRequestObject DeserializeRequestString(string requestString, IDictionary<string, string> headers, ProtocolVersion protocolVersion)
		{
			string controlSeparator = headers[HandshakeValuesMetadata.ControlSeparator];

			string[] split = requestString.Split(controlSeparator);

			return new MediumDeserializedRequestObject
			{
				ControlCommand = split[0],
				Parameters = HttpUtility.ParseQueryString(split[1])
			};
		}
	}
}
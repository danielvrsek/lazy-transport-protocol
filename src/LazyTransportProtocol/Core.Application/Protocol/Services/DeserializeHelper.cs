using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
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
	public static class DeserializeHelper
	{
		public static MediumDeserializedObject DeserializeRequestString(string requestString, AgreedHeadersDictionary headers, ProtocolVersion protocolVersion)
		{
			Contract.Requires(!String.IsNullOrWhiteSpace(requestString));
			Contract.Requires(headers != null);

			string controlSeparator = headers.Separator;

			string[] split = requestString.Split(controlSeparator);

			return new MediumDeserializedObject
			{
				ControlCommand = split[0],
				Body = split[1]
			};
		}
	}
}
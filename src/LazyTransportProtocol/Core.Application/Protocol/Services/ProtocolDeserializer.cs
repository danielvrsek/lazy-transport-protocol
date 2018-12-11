using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.Model;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Domain.Exceptions.Request;
using LazyTransportProtocol.Core.Domain.Exceptions.Response;
using System;
using System.Diagnostics.Contracts;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class ProtocolDeserializer
	{
		public static MediumDeserializedObject Deserialize(string requestString)
		{
			Contract.Requires(!String.IsNullOrWhiteSpace(requestString));

			string controlSeparator = ";";

			string[] split = requestString.Split(controlSeparator);

			if (split.Length != 3)
			{
				throw new ResponseStringDeserializationException();
			}

			return new MediumDeserializedObject
			{
				ControlCommand = split[0],
				RequestHeaders = split[1],
				Body = split[2]
			};
		}
	}
}
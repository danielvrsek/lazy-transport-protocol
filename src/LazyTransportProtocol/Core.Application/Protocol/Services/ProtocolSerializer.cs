using LazyTransportProtocol.Core.Application.Protocol.Model;
using LazyTransportProtocol.Core.Domain.Exceptions.Response;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class ProtocolSerializer
	{
		private const char separator = ';';

		public static string Serialize(string identifier, string headers, string body)
		{
			return identifier + separator + headers + separator + body;
		}

		public static MediumDeserializedObject Deserialize(string requestString)
		{
			string[] split = requestString.Split(separator);

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
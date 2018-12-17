using LazyTransportProtocol.Core.Application.Protocol.Model;
using System;
using System.Collections.Generic;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class ProtocolSerializer
	{
		public static IList<ArraySegment<byte>> Serialize(byte[] identifier, byte[] headers, byte[] body)
		{
			byte[] identifierLength = BitConverter.GetBytes(identifier.Length);
			byte[] headersLength = BitConverter.GetBytes(headers.Length);
			byte[] bodyLength = BitConverter.GetBytes(body.Length);

			return new List<ArraySegment<byte>>
			{
				new ArraySegment<byte>(identifierLength),
				new ArraySegment<byte>(headersLength),
				new ArraySegment<byte>(bodyLength),
				new ArraySegment<byte>(identifier),
				new ArraySegment<byte>(headers),
				new ArraySegment<byte>(body)
			};
		}

		public static MediumDeserializedObject Deserialize(byte[] requestSerialized)
		{
			int identifierLength = BitConverter.ToInt32(requestSerialized, 0);
			int headersLength = BitConverter.ToInt32(requestSerialized, 4);
			int bodyLength = BitConverter.ToInt32(requestSerialized, 8);

			int offset = 12;

			if (requestSerialized.Length - offset != identifierLength + headersLength + bodyLength)
			{
				throw new Exception("Request length is invalid.");
			}

			ArraySegment<byte> identifier = new ArraySegment<byte>(requestSerialized, offset, identifierLength);
			offset += identifierLength;
			ArraySegment<byte> headers = new ArraySegment<byte>(requestSerialized, offset, headersLength);
			offset += headersLength;
			ArraySegment<byte> body = new ArraySegment<byte>(requestSerialized, offset, bodyLength);

			return new MediumDeserializedObject
			{
				ControlCommand = identifier,
				RequestHeaders = headers,
				Body = body
			};
		}

		private static void CopyToArray(byte[] copyTo, byte[] copyFrom, ref int offset)
		{
			for (int i = 0; i < copyFrom.Length; i++, offset++)
			{
				copyTo[offset] = copyFrom[i];
			}
		}
	}
}
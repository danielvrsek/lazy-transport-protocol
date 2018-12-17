using System;

namespace LazyTransportProtocol.Core.Application.Protocol.Model
{
	public class MediumDeserializedObject
	{
		public ArraySegment<byte> ControlCommand { get; set; }

		public ArraySegment<byte> RequestHeaders { get; set; }

		public ArraySegment<byte> Body { get; set; }
	}
}
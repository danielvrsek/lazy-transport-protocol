using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Model
{
	public class MediumDeserializedObject
	{
		public string ControlCommand { get; set; }

		public string RequestHeaders { get; set; }

		public string Body { get; set; }
	}
}
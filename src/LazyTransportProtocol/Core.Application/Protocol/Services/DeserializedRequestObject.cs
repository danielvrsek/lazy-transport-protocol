using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class MediumDeserializedRequestObject
	{
		public string ControlCommand { get; set; }

		public NameValueCollection Parameters { get; set; }
	}
}
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Attributes
{
	public class IdentifierAttribute : Attribute
	{
		public string Identifier { get; set; }
	}
}
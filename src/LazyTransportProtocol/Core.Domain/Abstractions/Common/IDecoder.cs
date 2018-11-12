using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Domain.Abstractions.Common
{
	public interface IDecoder
	{
		string Decode(byte[] data);
	}
}
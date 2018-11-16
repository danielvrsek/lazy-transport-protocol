using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Domain.Exceptions
{
	public class ChunkDeserializationException : InvalidRequestException
	{
		public ChunkDeserializationException()
		{
		}

		public ChunkDeserializationException(Exception exception) : base(exception)
		{
		}
	}
}
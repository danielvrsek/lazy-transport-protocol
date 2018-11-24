using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Domain.Exceptions.Request
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
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Model
{
	public class Chunk
	{
		public int ChunkIdentifier { get; set; }

		public int ChunkIndex { get; set; }

		public ChunkTypeEnum ChunkType { get; set; }
	}
}
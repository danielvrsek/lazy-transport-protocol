using LazyTransportProtocol.Core.Application.Protocol.Model;
using LazyTransportProtocol.Core.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class ChunkParser
	{
		public static Chunk Parse(string value)
		{
			string[] flags = value.Split(" ").Where(x => !String.IsNullOrWhiteSpace(x)).ToArray();

			if (flags.Length != 3)
			{
				throw new ChunkDeserializationException();
			}

			if (!Int32.TryParse(flags[0], out int chunkIdentifier))
			{
				throw new ChunkDeserializationException();
			}

			if (!Int32.TryParse(flags[1], out int chunkIndex))
			{
				throw new ChunkDeserializationException();
			}

			ChunkTypeEnum chunkType;

			try
			{
				chunkType = ParseChunkType(flags[2][0]);
			}
			catch (ChunkTypeDeserializationException e)
			{
				throw new ChunkDeserializationException(e);
			}

			return new Chunk
			{
				ChunkIdentifier = chunkIdentifier,
				ChunkIndex = chunkIndex,
				ChunkType = chunkType
			};
		}

		public static ChunkTypeEnum ParseChunkType(char type)
		{
			switch (type)
			{
				case 'S':
					return ChunkTypeEnum.Start;

				case 'P':
					return ChunkTypeEnum.Part;

				case 'E':
					return ChunkTypeEnum.End;

				default:
					throw new ChunkTypeDeserializationException();
			}
		}
	}
}
using LazyTransportProtocol.Core.Application.Protocol.Model;
using LazyTransportProtocol.Core.Domain.Exceptions.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class ChunkParser
	{
		private readonly Dictionary<ChunkTypeEnum, char> _chunkIdentifierDictionary = new Dictionary<ChunkTypeEnum, char>();

		public ChunkParser()
		{
			_chunkIdentifierDictionary[ChunkTypeEnum.Start] = 'S';
			_chunkIdentifierDictionary[ChunkTypeEnum.Part] = 'P';
			_chunkIdentifierDictionary[ChunkTypeEnum.End] = 'E';
		}

		public Chunk Parse(string value)
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

		public ChunkTypeEnum ParseChunkType(char type)
		{
			try
			{
				return _chunkIdentifierDictionary.Single(kvp => kvp.Value == type).Key;
			}
			catch
			{
				throw new ChunkTypeDeserializationException();
			}
		}
	}
}
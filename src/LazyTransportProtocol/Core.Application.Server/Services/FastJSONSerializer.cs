using fastJSON;
using JWT;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Server.Services
{
	internal class FastJSONSerializer : IJsonSerializer
	{
		public T Deserialize<T>(string json)
		{
			return JSON.ToObject<T>(json);
		}

		public string Serialize(object obj)
		{
			return JSON.ToJSON(obj);
		}
	}
}
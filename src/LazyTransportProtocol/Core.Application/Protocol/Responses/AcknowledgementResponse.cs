using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Responses
{
	public class AcknowledgementResponse : IProtocolResponse
	{
		public bool IsSuccessful { get; set; }

		public int? ErrorCode { get; set; }

		public string Serialize(ProtocolVersion protocolVersion)
		{
			if (IsSuccessful)
			{
				return "OK";
			}

			return $"ERR {ErrorCode?.ToString() ?? String.Empty}".TrimEnd();
		}

		public void Deserialize(string data, ProtocolVersion protocolVersion)
		{
			if (data == "OK")
			{
				IsSuccessful = true;
			}
			else if (data.StartsWith("ERR"))
			{
				IsSuccessful = false;

				string[] flags = data.Split(' ');

				if (flags.Length > 1 && Int32.TryParse(flags[1], out int code))
				{
					ErrorCode = code;
				}
			}
			else
			{
				throw new Exception("Bad format.");
			}
		}
	}
}
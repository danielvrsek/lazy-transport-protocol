using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.ValueTypes
{
	public struct ProtocolVersion
	{
		public static ProtocolVersion Handshake { get; } = new ProtocolVersion("-1.0");
		public static ProtocolVersion V1_0 { get; } = new ProtocolVersion("1.0");

		public ProtocolVersion(string version)
		{
			string[] versions = version.Split('.');

			MajorVersion = Int32.Parse(versions[0]);
			MinorVersion = Int32.Parse(versions[1]);
		}

		public int MajorVersion { get; }

		public int MinorVersion { get; }

		public bool IsHandshake
		{
			get
			{
				return MajorVersion < 0;
			}
		}

		public override string ToString()
		{
			return $"{MajorVersion}.{MinorVersion}";
		}
	}
}
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Client.Exceptions
{
	public class InvalidCommandException : Exception
	{
		public InvalidCommandException()
		{

		}

		public InvalidCommandException(string message) : base(message)
		{

		}
	}
}

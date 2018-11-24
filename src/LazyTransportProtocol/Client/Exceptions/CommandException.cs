using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Client.Exceptions
{
	public class CommandException : Exception
	{
		public CommandException()
		{

		}

		public CommandException(string message) : base(message)
		{

		}
	}
}

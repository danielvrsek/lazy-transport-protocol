using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Domain.Exceptions
{
	public class CustomException : Exception
	{
		public CustomException()
		{
		}

		public CustomException(string message) : base(message)
		{
		}

		public CustomException(Exception exception) : base("See inner exception.", exception)
		{
		}
	}
}
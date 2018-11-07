using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Domain.Exceptions
{
	public abstract class CustomException : Exception
	{
		protected CustomException()
		{
		}

		protected CustomException(string message) : base(message)
		{
		}
	}
}
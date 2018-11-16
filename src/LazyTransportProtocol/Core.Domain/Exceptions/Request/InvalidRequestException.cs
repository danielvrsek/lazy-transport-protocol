using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Domain.Exceptions
{
	public class InvalidRequestException : CustomException
	{
		public InvalidRequestException()
		{
		}

		public InvalidRequestException(Exception exception) : base(exception)
		{
		}
	}
}
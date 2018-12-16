using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Domain.Exceptions.Request
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
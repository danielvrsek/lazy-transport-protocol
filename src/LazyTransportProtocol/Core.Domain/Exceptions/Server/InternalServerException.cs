using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Domain.Exceptions.Server
{
	public class InternalServerException : CustomException
	{
		public InternalServerException()
		{

		}

		public InternalServerException(string message) : base(message)
		{

		}

		public InternalServerException(Exception exception) : base(exception)
		{

		}
	}
}

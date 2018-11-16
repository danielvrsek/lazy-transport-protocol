using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Domain.Exceptions
{
	public class ValidationException : CustomException
	{
		protected ValidationException()
		{
		}

		public ValidationException(string typeName, string memberName) : base($"The member {memberName} on type {typeName} is not valid.")
		{
		}
	}
}
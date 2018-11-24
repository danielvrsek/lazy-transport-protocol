using LazyTransportProtocol.Core.Domain.Abstractions.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Client.Validators
{
	public class NotNullValidator : IValidator
	{
		public bool Validate(object value)
		{
			return value != null;
		}
	}
}

using LazyTransportProtocol.Core.Domain.Abstractions.Validators;
using System;

namespace LazyTransportProtocol.Core.Application.Validators
{
	public class NotNullOrEmptyValidator : IValidator<string>, IValidator
	{
		public bool Validate(string value)
		{
			return !String.IsNullOrEmpty(value);
		}

		public bool Validate(object value)
		{
			return Validate((string)value);
		}
	}
}
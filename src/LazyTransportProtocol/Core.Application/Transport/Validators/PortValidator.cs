using LazyTransportProtocol.Core.Domain.Abstractions.Validators;
using System;

namespace LazyTransportProtocol.Core.Application.Transport.Validators
{
	internal class PortValidator : IValidator
	{
		public bool Validate(object value)
		{
			int? intValue = value as int?;

			if (!intValue.HasValue)
			{
				return false;
			}

			if (intValue.Value < 0 || intValue.Value > Math.Pow(2, 16) - 1)
			{
				return false;
			}

			return true;
		}
	}
}
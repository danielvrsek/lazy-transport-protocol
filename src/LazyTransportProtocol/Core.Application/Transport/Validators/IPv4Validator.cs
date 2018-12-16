using LazyTransportProtocol.Core.Domain.Abstractions.Validators;
using System;

namespace LazyTransportProtocol.Core.Application.Transport.Validators
{
	internal class IPv4Validator : IValidator
	{
		public bool Validate(object value)
		{
			if (!(value is String stringValue))
			{
				return false;
			}

			string[] octets = stringValue.Split('.');

			if (octets.Length != 4)
			{
				return false;
			}

			foreach (string octet in octets)
			{
				if (!Int32.TryParse(octet, out int tmp))
				{
					return false;
				}

				if (tmp < 0 || tmp > 255)
				{
					return false;
				}
			}

			return true;
		}
	}
}
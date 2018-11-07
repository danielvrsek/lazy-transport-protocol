using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Domain.Abstractions.Validators
{
	public interface IValidator
	{
		bool Validate(object value);
	}
}
using LazyTransportProtocol.Core.Domain.Abstractions.Validators;
using System;

namespace LazyTransportProtocol.Core.Application.Validators
{
	internal class PredicateValidator<TValue> : IValidator<TValue>, IValidator
	{
		private readonly Predicate<TValue> _predicate;

		public PredicateValidator(Predicate<TValue> predicate)
		{
			_predicate = predicate;
		}

		public bool Validate(TValue value)
		{
			return _predicate(value);
		}

		public bool Validate(object value)
		{
			return Validate((TValue)value);
		}
	}
}
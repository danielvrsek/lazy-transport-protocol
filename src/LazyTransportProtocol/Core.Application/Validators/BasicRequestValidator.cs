using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions.Validators;
using LazyTransportProtocol.Core.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LazyTransportProtocol.Core.Application.Validators
{
	internal class BasicRequestValidator<TRequest> : IRequestValidator<TRequest>
		where TRequest : IRequest<IResponse>
	{
		private List<KeyValuePair<MemberInfo, IValidator>> _validators = new List<KeyValuePair<MemberInfo, IValidator>>();

		public IEnumerable<IValidator> this[MemberInfo memberInfo]
		{
			get
			{
				return _validators.Where(x => x.Key == memberInfo).Select(x => x.Value);
			}
		}

		public void AddValidator(MemberInfo memberInfo, IValidator validator)
		{
			_validators.Add(new KeyValuePair<MemberInfo, IValidator>(memberInfo, validator));
		}

		public void Validate(TRequest request)
		{
			foreach (KeyValuePair<MemberInfo, IValidator> validatorKvp in _validators)
			{
				object value = GetValue(validatorKvp.Key, request);

				if (!validatorKvp.Value.Validate(value))
				{
					throw new ValidationException(typeof(TRequest).Name, validatorKvp.Key.Name);
				}
			}
		}

		private static object GetValue(MemberInfo memberInfo, object forObject)
		{
			switch (memberInfo.MemberType)
			{
				case MemberTypes.Field:
					return ((FieldInfo)memberInfo).GetValue(forObject);

				case MemberTypes.Property:
					return ((PropertyInfo)memberInfo).GetValue(forObject);

				default:
					throw new NotImplementedException();
			}
		}
	}
}
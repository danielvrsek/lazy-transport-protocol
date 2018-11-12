using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using System;
using System.Linq.Expressions;

namespace LazyTransportProtocol.Core.Domain.Abstractions.Validators
{
	public interface IPipelineValidatorBuilder<T>
	{
		IPipelineValidatorBuilder<T> AddPropertyValidator<TValue>(Expression<Func<T, TValue>> propertyExpression, IValidator validator);

		IPipelineValidator<T> Build();
	}
}
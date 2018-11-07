using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using System;
using System.Linq.Expressions;

namespace LazyTransportProtocol.Core.Domain.Abstractions.Validators
{
	public interface IRequestValidatorBuilder<TRequest>
		where TRequest : IRequest<IResponse>
	{
		IRequestValidatorBuilder<TRequest> AddPropertyValidator<TValue>(Expression<Func<TRequest, TValue>> propertyExpression, IValidator validator);

		IRequestValidator<TRequest> Build();
	}
}
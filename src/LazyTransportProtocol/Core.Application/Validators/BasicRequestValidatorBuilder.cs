using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using LazyTransportProtocol.Core.Domain.Abstractions.Validators;
using System;
using System.Linq.Expressions;

namespace LazyTransportProtocol.Core.Application.Validators
{
	public class BasicRequestValidatorBuilder<TRequest> : IRequestValidatorBuilder<TRequest>
		where TRequest : IRequest<IResponse>
	{
		private BasicRequestValidator<TRequest> _requestValidator = new BasicRequestValidator<TRequest>();

		public IRequestValidatorBuilder<TRequest> AddPropertyValidator<TValue>(Expression<Func<TRequest, TValue>> expression, IValidator validator)
		{
			MemberExpression memberExpression = expression.Body as MemberExpression;

			_requestValidator.AddValidator(memberExpression.Member, validator);

			return this;
		}

		public IRequestValidator<TRequest> Build()
		{
			return _requestValidator;
		}
	}
}
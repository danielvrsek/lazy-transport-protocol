using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;

namespace LazyTransportProtocol.Core.Domain.Abstractions.Validators
{
	public interface IRequestValidator<TRequest>
		where TRequest : IRequest<IResponse>
	{
		void Validate(TRequest request);
	}
}
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;

namespace LazyTransportProtocol.Core.Domain.Abstractions.Validators
{
	public interface IPipelineValidator<T>
	{
		void Validate(T request);
	}
}
using LazyTransportProtocol.Core.Domain.Abstractions.Validators;

namespace LazyTransportProtocol.Client.Validators
{
	public class NotNullValidator : IValidator
	{
		public bool Validate(object value)
		{
			return value != null;
		}
	}
}
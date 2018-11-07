using LazyTransportProtocol.Core.Domain.Abstractions.Commands;

namespace LazyTransportProtocol.Core.Application.Transport.Abstractions.Commands
{
	public interface ITransportCommandHandler<TCommand> : ICommandHandler<TCommand>
		where TCommand : ITransportCommand
	{
	}
}
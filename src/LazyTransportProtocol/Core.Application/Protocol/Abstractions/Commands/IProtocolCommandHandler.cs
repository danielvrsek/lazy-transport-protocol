using LazyTransportProtocol.Core.Domain.Abstractions.Commands;

namespace LazyTransportProtocol.Core.Application.Protocol.Abstractions.Commands
{
	public interface IProtocolCommandHandler<TCommand> : ICommandHandler<TCommand>
		where TCommand : IProtocolCommand
	{
	}
}
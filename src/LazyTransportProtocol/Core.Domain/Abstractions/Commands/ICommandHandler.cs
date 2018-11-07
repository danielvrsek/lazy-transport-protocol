using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Domain.Abstractions.Commands
{
	/// <summary>
	/// Command handler
	/// </summary>
	public interface ICommandHandler<TCommand>
		where TCommand : ICommand
	{
		Task ExecuteCommandAsync(TCommand command, CancellationToken cancellationToken);

		void ExecuteCommand();
	}
}
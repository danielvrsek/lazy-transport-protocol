using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Domain.Abstractions.Flows
{
	public interface IFlow<TContext>
		where TContext : IFlowContext
	{
		void Start(TContext context);
	}
}
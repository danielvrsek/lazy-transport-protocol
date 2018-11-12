using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Flows;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Flows
{
	public class ConnectionFlowContext : IFlowContext
	{
		public IConnection ConnectionContext { get; set; }
	}
}
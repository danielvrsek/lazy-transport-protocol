using LazyTransportProtocol.Core.Application.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Handlers;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol
{
	public class ProtocolRequestExecutor : RequestExecutorBase
	{
		public override void Register()
		{
			Register<ListDirectoryClientRequest>((request) => new ListDirectoryRequestHandler().GetResponse((ListDirectoryClientRequest)request));
			Register<ChangeDirectoryClientRequest>((request) => new ChangeDirectoryRequestHandler().GetResponse((ChangeDirectoryClientRequest)request));
		}
	}
}
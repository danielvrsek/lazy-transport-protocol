using LazyTransportProtocol.Core.Application.Transport.Abstractions.Infrastructure;
using LazyTransportProtocol.Core.Application.Transport.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Transport.Responses
{
	public class SendDataResponse : ITransportResponse
	{
		public IConnectionContext ConnectionContext { get; }
	}
}
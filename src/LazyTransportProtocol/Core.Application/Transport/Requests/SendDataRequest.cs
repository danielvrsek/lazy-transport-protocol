using LazyTransportProtocol.Core.Application.Transport.Abstractions.Infrastructure;
using LazyTransportProtocol.Core.Application.Transport.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Transport.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Transport.Requests
{
	public class SendDataRequest : ITransportRequest<SendDataResponse>
	{
		public IConnectionContext ConnectionContext { get; }
	}
}
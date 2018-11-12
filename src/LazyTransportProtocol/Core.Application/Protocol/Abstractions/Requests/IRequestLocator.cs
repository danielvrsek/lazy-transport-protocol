using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests
{
	public interface IRequestLocator
	{
		IRequest<IResponse> Locate(string identifier);
	}
}
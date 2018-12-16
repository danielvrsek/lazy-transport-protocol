using LazyTransportProtocol.Core.Domain.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Domain.Abstractions.Requests
{
	/// <summary>
	/// Request requires response.
	/// Request when no response is required is <see cref="Commands.ICommand"/>
	/// </summary>
	/// <typeparam name="TResponse">Expected response type for the request</typeparam>
	public interface IRequest<out TResponse>
		where TResponse : IResponse
	{
	}
}
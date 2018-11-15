using LazyTransportProtocol.Core.Application.Protocol.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Abstractions.Services
{
	public interface IAuthenticationService
	{
		bool Authenticate(string username, string password);
	}
}
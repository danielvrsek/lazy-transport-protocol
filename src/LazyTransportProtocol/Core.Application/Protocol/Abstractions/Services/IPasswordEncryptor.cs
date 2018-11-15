using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Abstractions.Services
{
	public interface IPasswordEncryptor
	{
		string Encrypt(string password);
	}
}
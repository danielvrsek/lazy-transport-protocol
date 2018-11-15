using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class Sha1PasswordEncryptor : IPasswordEncryptor
	{
		public string Encrypt(string password)
		{
			return string.Join("", new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")).ToArray());
		}
	}
}
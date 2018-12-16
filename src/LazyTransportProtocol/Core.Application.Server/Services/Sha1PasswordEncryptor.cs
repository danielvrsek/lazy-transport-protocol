using LazyTransportProtocol.Core.Application.Server.Services.Abstractions;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Server.Services
{
	internal class Sha1PasswordEncryptor : IPasswordEncryptor
	{
		public string Encrypt(string password)
		{
			return string.Join("", new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")).ToArray());
		}
	}
}
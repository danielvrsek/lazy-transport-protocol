namespace LazyTransportProtocol.Core.Application.Server.Services.Abstractions
{
	public interface IPasswordEncryptor
	{
		string Encrypt(string password);
	}
}
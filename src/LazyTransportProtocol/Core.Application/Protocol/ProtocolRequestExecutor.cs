using LazyTransportProtocol.Core.Application.Protocol.Handlers;

namespace LazyTransportProtocol.Core.Application.Protocol
{
	public class ProtocolRequestExecutor : RequestExecutorBase
	{
		public override void Register()
		{
			Register(new HandshakeRequestHandler());
			Register(new AuthenticationRequestHandler());
			Register(new ListDirectoryRequestHandler());
			Register(new CreateUserRequestHandler());
		}
	}
}
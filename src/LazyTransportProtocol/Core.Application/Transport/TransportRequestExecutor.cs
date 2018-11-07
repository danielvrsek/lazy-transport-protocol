using LazyTransportProtocol.Core.Application.Abstractions;
using LazyTransportProtocol.Core.Application.Transport.Handlers;
using LazyTransportProtocol.Core.Application.Transport.Requests;
using LazyTransportProtocol.Core.Application.Transport.Validators;
using LazyTransportProtocol.Core.Application.Validators;

namespace LazyTransportProtocol.Core.Application.Transport
{
	public class TransportRequestExecutor : RequestExecutorBase
	{
		public override void Register()
		{
			Register<ConnectToServerRequest>((request) => new ConnectToServerRequestHandler().GetResponse(request))
				.AddValidator(
					new BasicRequestValidatorBuilder<ConnectToServerRequest>()
						.AddPropertyValidator((request) => request.IpAdress, new IPv4Validator())
						.AddPropertyValidator((request) => request.Port, new PortValidator())
						.Build()
				);

			//Register<SendDataRequest>((request) => new SendDataRequestHandler().GetResponse(request));
		}
	}
}
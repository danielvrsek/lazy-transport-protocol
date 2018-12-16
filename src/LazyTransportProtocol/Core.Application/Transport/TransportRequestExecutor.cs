using LazyTransportProtocol.Core.Application.Transport.Handlers;
using LazyTransportProtocol.Core.Application.Transport.Requests;
using LazyTransportProtocol.Core.Application.Transport.Validators;
using LazyTransportProtocol.Core.Application.Validators;

namespace LazyTransportProtocol.Core.Application.Transport
{
	internal class TransportRequestExecutor : RequestExecutorBase
	{
		public override void Register()
		{
			Register(new ListenToIncommingDataRequestHandler());
			Register(new ConnectToServerRequestHandler())
				.AddValidator(
					new BasicRequestValidatorBuilder<ConnectToServerRequest>()
						.AddPropertyValidator((request) => request.Port, new PortValidator())
						.Build())
				.OnException((ctx) =>
				{
					// Handle error
				});

			Register(new SendDataRequestHandler());
			Register(new EndConnectionRequestHandler());
		}
	}
}
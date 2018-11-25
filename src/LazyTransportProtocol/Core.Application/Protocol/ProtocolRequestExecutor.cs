using LazyTransportProtocol.Core.Application.Protocol.Handlers;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Application.Validators;
using System;

namespace LazyTransportProtocol.Core.Application.Protocol
{
	public class ProtocolRequestExecutor : RequestExecutorBase
	{
		public override void Register()
		{
			Register(new HandshakeRequestHandler());
			Register(new AuthenticationRequestHandler())
				.AddValidator(
					new BasicRequestValidatorBuilder<AuthenticationRequest>()
						.AddPropertyValidator(x => x.Username, new NotNullOrEmptyValidator())
						.AddPropertyValidator(x => x.Password, new NotNullOrEmptyValidator())
						.Build());
			Register(new ListDirectoryRequestHandler())
				.AddValidator(
					new BasicRequestValidatorBuilder<ListDirectoryClientRequest>()
						.AddPropertyValidator(x => x.Path, new NotNullOrEmptyValidator())
						.Build());
			Register(new CreateUserRequestHandler())
				.AddValidator(
					new BasicRequestValidatorBuilder<CreateUserRequest>()
						.AddPropertyValidator(x => x.Username, new NotNullOrEmptyValidator())
						.AddPropertyValidator(x => x.Password, new NotNullOrEmptyValidator())
						.Build());
			Register(new DeleteUserRequestHandler())
				.AddValidator(
					new BasicRequestValidatorBuilder<DeleteUserRequest>()
						.AddPropertyValidator(x => x.Username, new NotNullOrEmptyValidator())
						.Build());
			Register(new DownloadFileRequestHandler())
				.AddValidator(
					new BasicRequestValidatorBuilder<DownloadFileRequest>()
						.AddPropertyValidator(x => x.Filepath, new NotNullOrEmptyValidator())
						.AddPropertyValidator(x => x.Count, x => x > 0)
						.AddPropertyValidator(x => x.Offset, x => x >= 0)
						.Build());
			Register(new UploadFileRequestHandler())
				.AddValidator(
					new BasicRequestValidatorBuilder<UploadFileRequest>()
						.AddPropertyValidator(x => x.Path, new NotNullOrEmptyValidator())
						.Build());
		}
	}
}
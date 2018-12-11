using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Handlers;
using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Application.Validators;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Pipeline;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using System;
using System.Security.Authentication;

namespace LazyTransportProtocol.Core.Application.Protocol
{
	public class ProtocolRequestExecutor : RequestExecutorBase
	{
		public override IPipelineBuilder<TRequest> Register<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> requestHandler)
		{
			return base.Register(requestHandler)
				.AddPipelineAction((request) =>
				{
					AuthenticatedRequest<IProtocolResponse> authenticatedRequest = request as AuthenticatedRequest<IProtocolResponse>;

					if (authenticatedRequest != null)
					{
						ValidateAuthorization(authenticatedRequest);
					}
				});
		}

		private void ValidateAuthorization(AuthenticatedRequest<IProtocolResponse> request)
		{
			var response = Execute(new ValidateAuthenticationRequest
			{
				AuthenticationToken = request.AuthenticationToken
			});

			if (response.IsSuccessful)
			{
				request.AuthenticationContext = new SocketAuthenticationContext
				{
					Claims = response.Claims
				};
			}
			else
			{
				throw new AuthenticationException();
			}
		}

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

			Register(new CreateDirectoryRequestHandler());

			Register(new ValidateAuthenticationRequestHandler());
		}
	}
}
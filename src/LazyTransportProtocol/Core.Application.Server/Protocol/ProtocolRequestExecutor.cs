using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Requests.Abstractions;
using LazyTransportProtocol.Core.Application.Protocol.Responses.Abstractions;
using LazyTransportProtocol.Core.Application.Server.Protocol.Handlers;
using LazyTransportProtocol.Core.Application.Server.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Server.Protocol.Security.Authentication;
using LazyTransportProtocol.Core.Application.Validators;
using LazyTransportProtocol.Core.Domain.Abstractions.Pipeline;
using LazyTransportProtocol.Core.Domain.Abstractions.Requests;
using System.Security.Authentication;

namespace LazyTransportProtocol.Core.Application.Server.Protocol
{
	public class ProtocolRequestExecutor : RequestExecutorBase
	{
		public override IPipelineBuilder<TRequest> Register<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> requestHandler)
		{
			return base.Register(requestHandler)
				.AddToQueue((request) =>
				{
					if (request is IAuthenticatedRequest<IProtocolResponse> authenticatedRequest)
					{
						ValidateAuthentication(authenticatedRequest);
					}

					return request;
				});
		}

		private void ValidateAuthentication(IAuthenticatedRequest<IProtocolResponse> request)
		{
			var response = Execute(new ValidateAuthenticationRequest
			{
				AuthenticationToken = request.AuthenticationToken
			});

			if (!response.IsSuccessful)
			{
				throw new AuthenticationException();
			}

			AuthenticationContext ctx = new AuthenticationContext
			{
				Claims = response.Claims
			};

			AuthenticationContext.Insert(request, ctx);
			// TODO Authorization
		}

		public override void Register()
		{
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
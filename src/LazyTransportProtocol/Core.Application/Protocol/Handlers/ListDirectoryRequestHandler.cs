using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Model;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Domain.Exceptions.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Protocol.Handlers
{
	public class ListDirectoryRequestHandler : IProtocolRequestHandler<ListDirectoryClientRequest, ListDirectoryResponse>
	{
		public ListDirectoryResponse GetResponse(ListDirectoryClientRequest request)
		{
			AuthorizationService authorizationService = new AuthorizationService();
			if (!authorizationService.HasAccessToDirectory(request.AuthenticationContext, request.Path))
			{
				throw new AuthorizationException();
			}

			List<RemoteDirectory> remoteDirectories = GetDirectories(request.Path);
			List<RemoteFile> remoteFiles = GetFiles(request.Path);

			return new ListDirectoryResponse
			{
				RemoteDirectories = remoteDirectories,
				RemoteFiles = remoteFiles
			};
		}

		private List<RemoteFile> GetFiles(string path)
		{
			return IOService.GetFiles(path).Select(x => new RemoteFile
			{
				Filename = x
			}).ToList();
		}

		private List<RemoteDirectory> GetDirectories(string path)
		{
			return IOService.GetDirectories(path).Select(x => new RemoteDirectory
			{
				Name = x
			}).ToList();
		}

		public Task<ListDirectoryResponse> GetResponseAsync(ListDirectoryClientRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
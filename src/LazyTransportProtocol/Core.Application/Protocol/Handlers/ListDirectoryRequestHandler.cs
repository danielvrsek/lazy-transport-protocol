using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Extensions;
using LazyTransportProtocol.Core.Application.Protocol.Metadata;
using LazyTransportProtocol.Core.Application.Protocol.Model;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Domain.Exceptions.Authorization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
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

			IOService service = new IOService();

			return new ListDirectoryResponse
			{
				RemoteDirectories = GetDirectories(request.Path, service),
				RemoteFiles = GetFiles(request.Path, service)
			};
		}

		private List<RemoteFile> GetFiles(string path, IOService service)
		{
			return service.GetFiles(path).Select(x => new RemoteFile
			{
				Filename = x
			}).ToList();
		}
		private List<RemoteDirectory> GetDirectories(string path, IOService service)
		{
			return service.GetDirectories(path).Select(x => new RemoteDirectory
			{
				Name = x
			}).ToList();
		}
	}
}
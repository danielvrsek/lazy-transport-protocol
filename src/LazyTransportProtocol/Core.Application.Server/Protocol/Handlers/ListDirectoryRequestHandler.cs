using LazyTransportProtocol.Core.Application.Protocol.Model;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Server.Protocol.Handlers.Abstraction;
using LazyTransportProtocol.Core.Application.Server.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Server.Protocol.Handlers
{
	internal class ListDirectoryRequestHandler : IProtocolRequestHandler<ListDirectoryClientRequest, ListDirectoryResponse>
	{
		public ListDirectoryResponse GetResponse(ListDirectoryClientRequest request)
		{
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
				Filename = x,
				Size = (int)IOService.GetFileInfo(Path.Combine(path, x)).Length
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
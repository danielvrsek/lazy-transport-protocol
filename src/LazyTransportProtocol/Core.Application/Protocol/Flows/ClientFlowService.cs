using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Model;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LazyTransportProtocol.Core.Application.Protocol.Flow
{
	public class ClientFlowService
	{
		private readonly IRemoteRequestExecutor remoteExecutor = new SocketProtocolRequestExecutor();

		private string _currentFolder = "/";

		public void Connect(string ipAdress, int port)
		{
			remoteExecutor.Connect(new SocketConnectionParameters
			{
				IPAddress = ipAdress,
				Port = port
			});
		}

		public bool CreateNewUser(string username, string password)
		{
			AcknowledgementResponse response = remoteExecutor.Execute(new CreateUserRequest
			{
				Username = username,
				Password = password
			});

			return response.IsSuccessful;
		}

		public bool DeleteUser(string username)
		{
			AcknowledgementResponse response = remoteExecutor.Execute(new DeleteUserRequest
			{
				Username = username
			});

			return response.IsSuccessful;
		}

		public bool Authenticate(string username, string password)
		{
			AcknowledgementResponse response = remoteExecutor.Execute(new AuthenticationRequest
			{
				Username = username,
				Password = password
			});

			return response.IsSuccessful;
		}

		public void ListDirectory(string folder)
		{
			string path;

			if (String.IsNullOrEmpty(folder))
			{
				path = _currentFolder;
			}
			else
			{
				path = Path.IsPathRooted(folder) ? folder : Path.Combine(_currentFolder, folder);
			}

			var response = remoteExecutor.Execute(new ListDirectoryClientRequest
			{
				Path = path
			});

			List<string> names = response.RemoteDirectories.Select(x => x.Name).ToList();

			names.AddRange(response.RemoteFiles.Select(x => x.Filename));

			Console.WriteLine(String.Join(", ", names));
		}

		public void DownloadFile(string remoteFilepath, string localFilepath)
		{
			remoteFilepath = Path.IsPathRooted(remoteFilepath) ? remoteFilepath : Path.Combine(_currentFolder, remoteFilepath);

			int offset = 0;
			int count = 15000;

			DownloadFileResponse response;

			try
			{
				using (FileStream fs = File.OpenWrite(localFilepath))
				using (BinaryWriter bw = new BinaryWriter(fs))
				{
					do
					{
						response = remoteExecutor.Execute(new DownloadFileRequest
						{
							Filepath = remoteFilepath,
							Offset = offset,
							Count = count
						});

						byte[] data = Convert.FromBase64String(response.Data);

						bw.Write(data);

						offset += count;
					}
					while (response.HasNext);
				}
			}
			catch (IOException e)
			{
				throw new CustomException(e.Message);
			}
		}

		public void UploadFile(string localFile, string remoteFile)
		{
			throw new NotImplementedException();
		}

		public void ChangeDirectory(string folder)
		{
			_currentFolder = Path.IsPathRooted(folder) ? folder : Path.Combine(_currentFolder, folder);
		}

		public void Disconnect()
		{
			remoteExecutor.Disconnect();
		}
	}
}
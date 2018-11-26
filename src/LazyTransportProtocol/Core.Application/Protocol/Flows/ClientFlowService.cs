using LazyTransportProtocol.Core.Application.Helpers;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Model;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Protocol.Flow
{
	public class ClientFlowService
	{
		private readonly IRemoteRequestExecutor remoteExecutor = new SocketProtocolRequestExecutor();

		public string Host { get; private set; }

		public string Username { get; private set; }

		public string CurrentFolder { get; private set; } = "/";

		public void Connect(string ipAdress, int port)
		{
			remoteExecutor.Connect(new SocketConnectionParameters
			{
				IPAddress = ipAdress,
				Port = port
			});

			Host = ipAdress;
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

			if (response.IsSuccessful)
			{
				Username = username;
			}

			return response.IsSuccessful;
		}

		public void ListDirectory(string folder)
		{
			string path;

			if (String.IsNullOrEmpty(folder))
			{
				path = CurrentFolder;
			}
			else
			{
				path = Path.IsPathRooted(folder) ? folder : Path.Combine(CurrentFolder, folder);
			}

			var response = remoteExecutor.Execute(new ListDirectoryClientRequest
			{
				Path = path
			});

			List<string> names = response.RemoteDirectories.Select(x => x.Name).ToList();

			names.AddRange(response.RemoteFiles.Select(x => x.Filename));

			Console.WriteLine(String.Join(", ", names));
		}

		public void CreateDirectory(string path)
		{
			path = Path.IsPathRooted(path) ? path : Path.Combine(CurrentFolder, path);

			var response = remoteExecutor.Execute(new CreateDirectoryRequest
			{
				Path = path
			});
		}

		public void DownloadFile(string remoteFilepath, string localFilepath)
		{
			remoteFilepath = Path.IsPathRooted(remoteFilepath) ? remoteFilepath : Path.Combine(CurrentFolder, remoteFilepath);

			int index = 0;
			int offset = 0;
			int count = 15000;

			string remoteDirectory = Path.GetDirectoryName(remoteFilepath);
			string remoteFilename = Path.GetFileName(remoteFilepath);

			var lsResponse = remoteExecutor.Execute(new ListDirectoryClientRequest
			{
				Path = remoteDirectory
			});

			RemoteFile remoteFileInfo = lsResponse.RemoteFiles.FirstOrDefault(x => x.Filename == remoteFilename);

			if (remoteFileInfo == null)
			{
				throw new CustomException("File does not exist on the remote machine.");
			}

			Console.CursorVisible = false;

			WriteStatus("Downloading...", offset, remoteFileInfo.Size);

			DownloadFileResponse response;

			var sw = Stopwatch.StartNew();
			try
			{
				File.Delete(localFilepath);

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

						bw.Write(response.Data);

						offset += response.Data.Length;

						if (index % 10 == 0 || !response.HasNext)
						{
							WriteStatus("Downloading...", offset, remoteFileInfo.Size);
						}

						index++;
					}
					while (response.HasNext);
				}
			}
			catch (IOException e)
			{
				throw new CustomException(e.Message);
			}
			finally
			{
				Console.CursorVisible = true;
				Console.WriteLine();
				Console.WriteLine(sw.ElapsedMilliseconds);
			}
		}

		public void WriteStatus(string label, long downloaded, long totalSize)
		{
			int statusLength = 50;
			double percentage = (double)downloaded / totalSize;
			int completed = (int)(percentage * statusLength);

			label = $"{label} {percentage.ToString("P").PadLeft(8)}";
			string status = "[" + "".PadLeft(completed, '%').PadRight(statusLength, ' ') + "]";

			Console.SetCursorPosition(0, Console.CursorTop);
			Console.Write(label + "  " + status);
		}

		public void UploadFile(string localFilepath, string remoteFilepath)
		{
			remoteFilepath = Path.IsPathRooted(remoteFilepath) ? remoteFilepath : Path.Combine(CurrentFolder, remoteFilepath);

			int count = 15000;
			int offset = 0;
			int index = 0;

			FileInfo localFileInfo = new FileInfo(localFilepath);
			long localFileSize = localFileInfo.Length;

			Console.CursorVisible = false;
			WriteStatus("Uploading...", 0, localFileSize);

			AcknowledgementResponse response;
			byte[] buffer;
			object obj = new object();

			try
			{
				using (FileStream fs = File.OpenRead(localFilepath))
				using (BinaryReader br = new BinaryReader(fs))
				{
					do
					{
						buffer = br.ReadBytes(count);

						response = remoteExecutor.Execute(new UploadFileRequest
						{
							Path = remoteFilepath,
							Data = buffer,
							Offset = offset
						});

						offset += buffer.Length;

						if (!response.IsSuccessful)
						{
							throw new CustomException("Error while uploading.");
						}

						if (index % 10 == 0 || buffer.Length != count)
						{
							WriteStatus("Uploading...", offset, localFileSize);
						}
					}
					while (buffer.Length == count);
				}
			}
			catch (IOException e)
			{
				throw new CustomException(e.Message);
			}
			finally
			{
				Console.CursorVisible = true;
				Console.WriteLine();
			}
		}

		public void ChangeDirectory(string folder)
		{
			CurrentFolder = Path.IsPathRooted(folder) ? folder : Path.Combine(CurrentFolder, folder);
		}

		public void Disconnect()
		{
			remoteExecutor.Disconnect();
		}
	}
}
using LazyTransportProtocol.Core.Application.IO;
using LazyTransportProtocol.Core.Application.Protocol;
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
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Client.Services
{
	public class ClientFlowService
	{
		private SocketConnectionParameters connectionParameters;
		private readonly IRemoteRequestExecutor remoteExecutor = new SocketProtocolRequestExecutor();
		private readonly List<IRemoteRequestExecutor> helperExecutors = new List<IRemoteRequestExecutor>();

		public string Host { get; private set; }

		public string Username { get; private set; }

		public string CurrentFolder { get; private set; } = "/";

		private string authenticationToken = null;

		public void Connect(string ipAdress, int port)
		{
			IPAddress ip = IPAddress.Parse(ipAdress);

			connectionParameters = new SocketConnectionParameters
			{
				IPAddress = ip,
				Port = port
			};

			remoteExecutor.Connect(connectionParameters);

			Host = ipAdress;
		}

		public bool CreateNewUser(string username, string password)
		{
			AcknowledgementResponse response = remoteExecutor.Execute(new CreateUserRequest
			{
				Username = username,
				Password = password,
				AuthenticationToken = authenticationToken
			});

			return response.IsSuccessful;
		}

		public bool DeleteUser(string username)
		{
			AcknowledgementResponse response = remoteExecutor.Execute(new DeleteUserRequest
			{
				Username = username,
				AuthenticationToken = authenticationToken
			});

			return response.IsSuccessful;
		}

		public bool Authenticate(string username, string password)
		{
			AuthenticationResponse response = remoteExecutor.Execute(new AuthenticationRequest
			{
				Username = username,
				Password = password
			});

			if (response.IsSuccessful)
			{
				Username = username;
				authenticationToken = response.AuthenticationToken;
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
				Path = path,
				AuthenticationToken = authenticationToken
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
				Path = path,
				AuthenticationToken = authenticationToken
			});
		}

		private IRemoteRequestExecutor[] GetHelperRequestExecutors(int count)
		{
			if (helperExecutors.Count < count)
			{
				for (int i = 0; i < count - helperExecutors.Count; i++)
				{
					IRemoteRequestExecutor executor = new SocketProtocolRequestExecutor();
					executor.Connect(connectionParameters);
					helperExecutors.Add(executor);
				}
			}

			return helperExecutors.Take(count).ToArray();
		}

		public void DownloadFileSingleConnection(string remoteFilepath, string localFilepath)
		{
			remoteFilepath = Path.IsPathRooted(remoteFilepath) ? remoteFilepath : Path.Combine(CurrentFolder, remoteFilepath);

			int offset = 0;
			int count = 512 * 1024;

			string remoteDirectory = Path.GetDirectoryName(remoteFilepath);
			string remoteFilename = Path.GetFileName(remoteFilepath);

			var lsResponse = remoteExecutor.Execute(new ListDirectoryClientRequest
			{
				Path = remoteDirectory,
				AuthenticationToken = authenticationToken
			});

			RemoteFile remoteFileInfo = lsResponse.RemoteFiles.FirstOrDefault(x => x.Filename == remoteFilename);

			if (remoteFileInfo == null)
			{
				throw new CustomException("File does not exist on the remote machine.");
			}

			var sw = Stopwatch.StartNew();

			using (ConsoleStatusWriter statusWriter = new ConsoleStatusWriter("Downloading...", remoteFileInfo.Size))
			{
				if (File.Exists(localFilepath))
				{
					File.Delete(localFilepath);
				}

				DownloadFileResponse response;

				using (FileStream fs = File.OpenWrite(localFilepath))
				using (BinaryWriter bw = new BinaryWriter(fs))
				{
					do
					{
						response = remoteExecutor.Execute(new DownloadFileRequest
						{
							Filepath = remoteFilepath,
							Offset = offset,
							Count = count,
							AuthenticationToken = authenticationToken
						});

						bw.Write(response.Data);

						offset += response.Data.Length;

						statusWriter.Update(response.Data.Length);
					}
					while (response.HasNext);
				}
			}

			Console.WriteLine("Completed in: " + sw.ElapsedMilliseconds / (double)1000 + "s");
		}

		public void DownloadFile(string remoteFilepath, string localFilepath)
		{
			remoteFilepath = Path.IsPathRooted(remoteFilepath) ? remoteFilepath : Path.Combine(CurrentFolder, remoteFilepath);

			string remoteDirectory = Path.GetDirectoryName(remoteFilepath);
			string remoteFilename = Path.GetFileName(remoteFilepath);

			var lsResponse = remoteExecutor.Execute(new ListDirectoryClientRequest
			{
				Path = remoteDirectory,
				AuthenticationToken = authenticationToken
			});

			RemoteFile remoteFileInfo = lsResponse.RemoteFiles.FirstOrDefault(x => x.Filename == remoteFilename);

			if (remoteFileInfo == null)
			{
				throw new CustomException("File does not exist on the remote machine.");
			}

			int partLength = 512 * 1024;
			ParallelFileDownloader parallelFileDownloader = new ParallelFileDownloader(remoteFileInfo.Size, partLength);

			Stopwatch sw = Stopwatch.StartNew();

			using (ParallelFileWriter parallelFileWriter = new ParallelFileWriter(localFilepath))
			using (ConsoleStatusWriter statusWriter = new ConsoleStatusWriter("Downloading...", remoteFileInfo.Size))
			{
				parallelFileDownloader.FilePartDownloadedEvent += (partNumber, data) => ParallelFileDownloader_FilePartDownloadedEvent(parallelFileWriter, statusWriter, partNumber, data);

				IRemoteRequestExecutor[] executors = GetHelperRequestExecutors(4);

				foreach (var helperExecutor in executors)
				{
					parallelFileDownloader.StartNew((offset, count) => helperExecutor.Execute(new DownloadFileRequest
					{
						AuthenticationToken = authenticationToken,
						Filepath = remoteFilepath,
						Offset = offset,
						Count = count
					}).Data);
				}

				parallelFileDownloader.Wait();
			}

			Console.WriteLine("Completed in: " + sw.ElapsedMilliseconds / (double)1000 + "s");
		}

		private void ParallelFileDownloader_FilePartDownloadedEvent(ParallelFileWriter writer, ConsoleStatusWriter statusWriter, int partNumber, byte[] data)
		{
			writer.WritePart(partNumber, data);
			statusWriter.Update(data.Length);
		}

		public void UploadFile(string localFilepath, string remoteFilepath)
		{
			remoteFilepath = Path.IsPathRooted(remoteFilepath) ? remoteFilepath : Path.Combine(CurrentFolder, remoteFilepath);

			int count = 15000;
			int offset = 0;
			int index = 0;

			FileInfo localFileInfo = new FileInfo(localFilepath);
			long localFileSize = localFileInfo.Length;

			ConsoleStatusWriter statusWriter = new ConsoleStatusWriter("Uploading...", localFileSize);

			AcknowledgementResponse response;
			byte[] buffer;

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
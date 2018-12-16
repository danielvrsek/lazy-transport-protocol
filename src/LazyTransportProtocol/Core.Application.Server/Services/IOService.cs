using LazyTransportProtocol.Core.Application.Server.Configuration;
using System;
using System.IO;
using System.Linq;

namespace LazyTransportProtocol.Core.Application.Server.Services
{
	internal class IOService
	{
		private static readonly string _currentDirectory = Directory.GetCurrentDirectory();

		/// <summary>
		/// Transforms path from request into absolute system path
		/// </summary>
		public static string TransformPath(string path)
		{
			string rootFolder = GetAbsoluteRootFolder();

			if (Path.IsPathRooted(path))
			{
				string pathRoot = Path.GetPathRoot(path);
				path = path.Substring(pathRoot.Length);
			}

			return Path.Combine(rootFolder, path);
		}

		public static string GetAbsoluteRootFolder()
		{
			string rootFolder = ServerConfiguration.Instance().RootFolder;

			if (!Path.IsPathRooted(rootFolder))
			{
				rootFolder = Path.Combine(_currentDirectory, rootFolder);
			}

			return Uri.UnescapeDataString(rootFolder);
		}

		public static string CreateDirectory(string path)
		{
			string systemPath = TransformPath(path);

			DirectoryInfo directoryInfo = Directory.CreateDirectory(systemPath);

			return directoryInfo.FullName;
		}

		public static void MoveDirectory(string oldPath, string newPath)
		{
			string oldSystemPath = TransformPath(oldPath);
			string newSystemPath = TransformPath(newPath);

			Directory.Move(oldSystemPath, newSystemPath);
		}

		public static void DeleteDirectory(string path, bool recursive = true)
		{
			string systemPath = TransformPath(path);

			Directory.Delete(systemPath, recursive);
		}

		public static bool FileExists(string filepath)
		{
			string systemPath = TransformPath(filepath);

			return File.Exists(systemPath);
		}

		public static byte[] ReadFile(string fullpath, int offset, int count)
		{
			string systemPath = TransformPath(fullpath);

			byte[] buffer = new byte[count];
			int bytesRead = 0;

			using (FileStream fs = File.OpenRead(systemPath))
			using (BinaryReader binaryReader = new BinaryReader(fs))
			{
				fs.Position = offset;

				bytesRead = binaryReader.Read(buffer, 0, count);
			}

			if (bytesRead < buffer.Length)
			{
				Array.Resize(ref buffer, bytesRead);
			}

			return buffer;
		}

		public static void AppendFile(string filePath, byte[] data, int offset, bool createIfNotExists = true)
		{
			string systemPath = TransformPath(filePath);

			if (!createIfNotExists && !File.Exists(systemPath))
			{
				throw new Exception("File does not exist.");
			}

			using (FileStream fs = File.OpenWrite(systemPath))
			{
				fs.Position = offset;
				fs.Write(data, 0, data.Length);
			}
		}

		public static void CopyFile(string oldFilePath, string newFilePath)
		{
			string oldSystemPath = TransformPath(oldFilePath);
			string newSystemPath = TransformPath(newFilePath);

			File.Copy(oldSystemPath, newSystemPath);
		}

		public static void MoveFile(string oldFilePath, string newFilePath)
		{
			string oldSystemPath = TransformPath(oldFilePath);
			string newSystemPath = TransformPath(newFilePath);

			File.Move(oldSystemPath, newSystemPath);
		}

		public static void DeleteFile(string filePath)
		{
			string systemPath = TransformPath(filePath);

			File.Delete(systemPath);
		}

		public static string[] GetDirectories(string path)
		{
			string systemPath = TransformPath(path);

			return Directory.EnumerateDirectories(systemPath).Select(x => Path.GetFileName(x)).ToArray();
		}

		public static string[] GetFiles(string path)
		{
			string systemPath = TransformPath(path);

			return Directory.EnumerateFiles(systemPath).Select(x => Path.GetFileName(x)).ToArray();
		}

		public static FileInfo GetFileInfo(string filePath)
		{
			string systemPath = TransformPath(filePath);

			return new FileInfo(systemPath);
		}
	}
}
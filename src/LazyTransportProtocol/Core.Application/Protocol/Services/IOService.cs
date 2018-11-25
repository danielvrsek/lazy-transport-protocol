using LazyTransportProtocol.Core.Application.Protocol.Configuration;
using LazyTransportProtocol.Core.Domain.Exceptions.Authorization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class IOService
	{
		/// <summary>
		/// Transforms path from request into system path
		/// </summary>
		/// <returns></returns>
		public static string TransformPath(string path)
		{
			string rootFolder = ServerConfiguration.RootFolder;
			path = Uri.UnescapeDataString(path);

			if (Path.IsPathRooted(path))
			{
				string pathRoot = Path.GetPathRoot(path);
				path = path.Substring(pathRoot.Length);
			}

			string combined = Path.Combine(rootFolder, path);

			return combined;
		}

		public static string CreateDirectory(string path, string directoryName)
		{
			string systemPath = TransformPath(path);

			if (directoryName.Contains('\\') || directoryName.Contains('/'))
			{
				throw new ArgumentException("Invalid directory name.");
			}

			string fullPath = Path.Combine(systemPath, directoryName);

			DirectoryInfo directoryInfo = Directory.CreateDirectory(fullPath);

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

		public static Span<byte> ReadFile(string fullpath, int offset, int count)
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

			return new Span<byte>(buffer, 0, bytesRead);
		}

		public static void AppendFile(string filePath, byte[] data, bool createIfNotExists = true)
		{
			string systemPath = TransformPath(filePath);

			if (!createIfNotExists && !File.Exists(systemPath))
			{
				throw new Exception("File does not exist.");
			}

			using (FileStream fs = File.OpenWrite(systemPath))
			{
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

			return Directory.EnumerateDirectories(systemPath).ToArray();
		}

		public static string[] GetFiles(string path)
		{
			string systemPath = TransformPath(path);

			return Directory.EnumerateFiles(systemPath).ToArray();
		}

	}
}

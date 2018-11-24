using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class IOService
	{
		public string CreateDirectory(string path, string directoryName)
		{
			string fullPath = Path.Combine(path, directoryName);

			DirectoryInfo directoryInfo = Directory.CreateDirectory(fullPath);

			return directoryInfo.FullName;
		}

		public void MoveDirectory(string oldPath, string newPath)
		{
			Directory.Move(oldPath, newPath);
		}

		public void DeleteDirectory(string path, bool recursive = true)
		{
			Directory.Delete(path, recursive);
		}

		public byte[] ReadFile(string fullpath, int offset, int count)
		{
			byte[] buffer = new byte[count];
			int bytesRead = 0;

			using (FileStream fs = File.OpenRead(fullpath))
			using (BinaryReader binaryReader = new BinaryReader(fs))
			{
				fs.Position = offset;

				bytesRead = binaryReader.Read(buffer, 0, count);
			}

			Array.Resize(ref buffer, bytesRead);

			return buffer;
		}

		public void AppendFile(string filePath, byte[] data, bool createIfNotExists = true)
		{
			if (!createIfNotExists && !File.Exists(filePath))
			{
				throw new Exception("File does not exist.");
			}

			using (StreamWriter sw = new StreamWriter(filePath, true))
			{
				sw.Write(data);
			}
		}

		public void CopyFile(string oldFilePath, string newFilePath)
		{
			File.Copy(oldFilePath, newFilePath);
		}

		public void MoveFile(string oldFilePath, string newFilePath)
		{
			File.Move(oldFilePath, newFilePath);
		}

		public void DeleteFile(string filePath)
		{
			File.Delete(filePath);
		}

	}
}

using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Services;
using LazyTransportProtocol.Core.Application.Protocol.Configuration;
using LazyTransportProtocol.Core.Application.Protocol.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class UserSecretAccessor : IUserSecretAccessor
	{
		private readonly string _filename;
		private readonly string _path;

		private string FullPath
		{
			get
			{
				return Path.Combine(_path, _filename);
			}
		}

		#region ..ctors

		public UserSecretAccessor() : this(ServerConfiguration.UserSecretFilename)
		{
		}

		public UserSecretAccessor(string filename) : this(filename, ServerConfiguration.UserSecretFilePath)
		{
		}

		public UserSecretAccessor(string filename, string path)
		{
			this._filename = filename;
			this._path = path;
		}

		#endregion ..ctors

		#region Public members

		public UserSecret GetSecretForUsername(string username)
		{
			return GetAllLines().SingleOrDefault(x => x.Username == username);
		}

		public UserSecret InsertNewSercret(UserSecret secretInfo)
		{
			EnsureFileExists();

			using (StreamWriter sw = new StreamWriter(FullPath, true))
			{
				sw.WriteLine(SerializeSecret(secretInfo));
			}

			return secretInfo;
		}

		#endregion Public members

		#region Private members

		private UserSecret[] GetAllLines()
		{
			EnsureFileExists();

			string fileContents = null;

			using (StreamReader sr = new StreamReader(FullPath))
			{
				fileContents = sr.ReadToEnd();
			}

			string[] lines = fileContents.Split(Environment.NewLine);

			return lines.Where(line => !line.StartsWith("#") && !String.IsNullOrWhiteSpace(line)).Select(line => ParseLine(line)).ToArray();
		}

		private UserSecret ParseLine(string line)
		{
			string[] parameters = line.Split(';');

			return new UserSecret
			{
				Username = parameters[0],
				Password = parameters[1]
			};
		}

		private string SerializeSecret(UserSecret userSecret)
		{
			return $"{userSecret.Username};{userSecret.Password}";
		}

		private void EnsureFileExists()
		{
			if (!File.Exists(FullPath))
			{
				using (StreamWriter file = new StreamWriter(FullPath))
				{
					file.WriteLine("#########");
				}
			}
		}

		#endregion Private members
	}
}
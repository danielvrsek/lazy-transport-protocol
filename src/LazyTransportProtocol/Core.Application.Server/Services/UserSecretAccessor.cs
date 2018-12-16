using LazyTransportProtocol.Core.Application.Server.Configuration;
using LazyTransportProtocol.Core.Application.Server.Protocol.Model;
using LazyTransportProtocol.Core.Application.Server.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LazyTransportProtocol.Core.Application.Server.Services
{
	internal class UserSecretAccessor : IUserSecretAccessor
	{
		private readonly string _filePath;

		#region ..ctors

		public UserSecretAccessor() : this(ServerConfiguration.Instance().UserSecretFilepath)
		{
		}

		public UserSecretAccessor(string filePath)
		{
			_filePath = filePath;
		}

		#endregion ..ctors

		#region Public members

		public UserSecret GetSecretForUsername(string username)
		{
			return GetAllLines().SingleOrDefault(x => x.Username == username);
		}

		public UserSecret InsertNewSercret(UserSecret secretInfo)
		{
			using (StreamWriter sw = new StreamWriter(_filePath, true))
			{
				sw.WriteLine(SerializeSecret(secretInfo));
			}

			return secretInfo;
		}

		public void DeleteSecret(string username)
		{
			string[] lines = ReadFile();

			string[] newLines = lines.Where(x => !x.StartsWith(username)).ToArray();

			using (StreamWriter sw = new StreamWriter(_filePath, false))
			{
				foreach (string line in newLines)
				{
					sw.WriteLine(line);
				}
			}
		}

		public UserSecret ModifySecret(UserSecret userSecret)
		{
			DeleteSecret(userSecret.Username);
			InsertNewSercret(userSecret);

			return userSecret;
		}

		#endregion Public members

		#region Private members

		private UserSecret[] GetAllLines()
		{
			string[] lines = ReadFile();

			return lines.Where(line => !line.StartsWith("#") && !String.IsNullOrWhiteSpace(line)).Select(line => ParseLine(line)).ToArray();
		}

		private string[] ReadFile()
		{
			List<string> lines = new List<string>();

			using (StreamReader sr = new StreamReader(_filePath))
			{
				string line = sr.ReadLine();

				while (line != null)
				{
					lines.Add(line);
					line = sr.ReadLine();
				}
			}

			return lines.ToArray();
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

		#endregion Private members
	}
}
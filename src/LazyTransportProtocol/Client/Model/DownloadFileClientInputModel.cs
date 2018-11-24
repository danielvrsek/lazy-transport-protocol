using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LazyTransportProtocol.Client.Model
{
	public class DownloadFileClientInputModel
	{
		public string RemoteFile { get; set; }

		private string _localFile;

		public string LocalFile
		{
			get
			{
				if (_localFile == null)
				{
					return _localFile = Path.GetFileName(RemoteFile);
				}

				return _localFile;
			}
			set
			{
				_localFile = value;
			}
		}

	}
}

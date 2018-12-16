using System.IO;

namespace LazyTransportProtocol.Client.Model
{
	public class UploadFileClientInputModel
	{
		public string LocalFile { get; set; }

		private string _remoteFile;

		public string RemoteFile
		{
			get
			{
				if (_remoteFile == null)
				{
					return _remoteFile = Path.GetFileName(LocalFile);
				}

				return _remoteFile;
			}
			set
			{
				_remoteFile = value;
			}
		}
	}
}
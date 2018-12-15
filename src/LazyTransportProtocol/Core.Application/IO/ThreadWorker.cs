using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.IO
{
	internal class ThreadWorker
	{
		private readonly object _lock = new object();

		private readonly Dictionary<int, byte[]> _partDataDictionary = new Dictionary<int, byte[]>();

		private readonly BinaryWriter _binaryWriter;
		private readonly ManualResetEventSlim _lockEvent;
		private readonly CancellationToken _token;

		private int _currentPartNumber = 0;
		private int _lastPartNumber = -1;
		private int _queueLength = 0;
		private readonly int _timeout;

		public bool EndedSuccessfully { get; private set; } = true;

		public ThreadWorker(BinaryWriter binaryWriter, ManualResetEventSlim lockEvent, CancellationToken token, int timeout = 10000)
		{
			_binaryWriter = binaryWriter;
			_lockEvent = lockEvent;
			_token = token;
			_timeout = timeout;
		}

		public void AddPartData(int partNumber, byte[] data)
		{
			lock (_lock)
			{
				_queueLength += data.Length;
				_partDataDictionary.Add(partNumber, data);

				if (partNumber > _lastPartNumber)
				{
					_lastPartNumber = partNumber;
				}
			}
		}

		public void BeginWrite()
		{
			_lockEvent.Reset();

			while (!_token.IsCancellationRequested || !IsAllDataWritten())
			{
				TryWriteDataContinuously();

				_binaryWriter.Flush();

				// More data might have come while flushing
				if (_token.IsCancellationRequested && !IsAllDataWritten())
				{
					TryWriteDataContinuously();
					_binaryWriter.Flush();

					// If there are still no data available
					if (!IsAllDataWritten())
					{
						EndedSuccessfully = false;
						break;
					}
				}
				Thread.Sleep(1);
			}

			_lockEvent.Set();
		}

		private void RemovePartData(int partNumber)
		{
			lock (_lock)
			{
				int length = _partDataDictionary[partNumber].Length;
				_queueLength -= length;
				_partDataDictionary[partNumber] = null;
			}
		}

		private bool IsAllDataWritten()
		{
			lock (_lock)
			{
				return _lastPartNumber != -1 && _partDataDictionary[_lastPartNumber] == null;
			}
		}

		private void TryWriteDataContinuously()
		{
			byte[] data;

			for (; true; _currentPartNumber++)
			{
				data = GetDataToWrite(_currentPartNumber);

				if (data == null)
				{
					break;
				}

				bool success = WriteFilePart(data);
				if (success)
				{
					RemovePartData(_currentPartNumber);
				}
			}
		}

		private byte[] GetDataToWrite(int partNumber)
		{
			lock (_lock)
			{
				if (!_partDataDictionary.TryGetValue(partNumber, out byte[] data))
				{
					return null;
				}

				return data;
			}
		}

		private bool WriteFilePart(byte[] data)
		{
			try
			{
				_binaryWriter.Write(data);
			}
			catch
			{
				return false;
			}

			return true;
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.IO
{
	public class ParallelFileWriter : IDisposable
	{
		private readonly Dictionary<int, byte[]> _partDataDictionary = new Dictionary<int, byte[]>();
		private readonly Dictionary<int, Action<bool>> _callbackDictionary = new Dictionary<int, Action<bool>>();
		private readonly object _partDataLock = new object();
		private readonly object _callbackLock = new object();

		private readonly ReaderWriterLock _rwl = new ReaderWriterLock();
		private readonly Thread _workerThread;
		private volatile int _queueLength = 0;
		private CancellationTokenSource _cancellationTokenSource;

		private readonly FileStream _fileStream;
		private readonly BinaryWriter _binaryWriter;

		private readonly int _timeout = 0;

		public ParallelFileWriter(string filePath, int timeout = 10000)
		{
			_timeout = timeout;

			_fileStream = File.OpenWrite(filePath);
			_binaryWriter = new BinaryWriter(_fileStream);

			_cancellationTokenSource = new CancellationTokenSource();

			_workerThread = new Thread((obj) => BeginWrite(obj));
			_workerThread.Start(_cancellationTokenSource.Token);
		}

		public Task<bool> WritePartAsync(int partNumber, byte[] data)
		{
			TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(_timeout);

			cancellationTokenSource.Token.Register(() => taskCompletionSource.TrySetResult(false));

			AddCallback(partNumber, (success) => taskCompletionSource.TrySetResult(success));
			AddPartData(partNumber, data);

			return taskCompletionSource.Task;
		}

		private void AddCallback(int partNumber, Action<bool> action)
		{
			lock (_callbackLock)
			{
				_callbackDictionary.Add(partNumber, action);
			}
		}

		private void AddPartData(int partNumber, byte[] data)
		{
			lock (_partDataLock)
			{
				_queueLength += data.Length;
				_partDataDictionary.Add(partNumber, data);
			}
		}

		private void RemovePartData(int partNumber)
		{
			lock (_partDataLock)
			{
				int length = _partDataDictionary[partNumber].Length;
				_queueLength -= length;
				_partDataDictionary[partNumber] = null;
			}
		}

		private void BeginWrite(object obj)
		{
			CancellationToken token = (CancellationToken)obj;
			int currentPartNumber = -1;

			while (!token.IsCancellationRequested)
			{
				_rwl.AcquireWriterLock(_timeout);
				byte[] data = GetNextDataToWrite(ref currentPartNumber);

				while (data != null)
				{
					bool success = WriteFilePart(data);

					if (success)
					{
						RemovePartData(currentPartNumber);
					}

					Action<bool> callback = _callbackDictionary[currentPartNumber];
					// Spustit jako task, jelikoz se jedna o synchronni operaci
					Task.Run(() => callback(success));

					data = GetNextDataToWrite(ref currentPartNumber);
				}

				_binaryWriter.Flush();

				_rwl.ReleaseWriterLock();
				Thread.Sleep(1);
			}
		}

		private byte[] GetNextDataToWrite(ref int partNumber)
		{
			Dictionary<int, byte[]> dictionarySnapshot;

			lock (_partDataLock)
			{
				dictionarySnapshot = _partDataDictionary.ToDictionary(x => x.Key, x => x.Value);
			}

			if (!dictionarySnapshot.TryGetValue(partNumber + 1, out byte[] data))
			{
				return null;
			}

			partNumber += 1;
			return data;
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

		public void Dispose()
		{
			_cancellationTokenSource.Cancel();

			_rwl.AcquireWriterLock(_timeout);
			_workerThread.Join();

			_binaryWriter.Dispose();
			_fileStream.Dispose();
			_rwl.ReleaseWriterLock();
		}
	}
}

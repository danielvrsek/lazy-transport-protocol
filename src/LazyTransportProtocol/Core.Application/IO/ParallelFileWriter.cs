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
		private readonly ReaderWriterLock _rwl = new ReaderWriterLock();
		private readonly Thread _workerThread;
		private readonly ThreadWorker _threadWorker;
		private CancellationTokenSource _cancellationTokenSource;
		private ManualResetEventSlim _startEvent;
		private ManualResetEventSlim _cancellationEvent;

		private readonly string _filePath;
		private readonly string _tempFilePath;
		private readonly FileStream _fileStream;
		private readonly BinaryWriter _binaryWriter;

		private readonly int _timeout = 0;

		public ParallelFileWriter(string filePath, int timeout = 10000)
		{
			_tempFilePath = Path.GetTempFileName();
			_filePath = filePath;
			_timeout = timeout;

			_fileStream = File.OpenWrite(_tempFilePath);
			_binaryWriter = new BinaryWriter(_fileStream);
			_cancellationTokenSource = new CancellationTokenSource();
			_startEvent = new ManualResetEventSlim(false);
			_cancellationEvent = new ManualResetEventSlim(false);

			_threadWorker = new ThreadWorker(_binaryWriter, _cancellationEvent, _cancellationTokenSource.Token, timeout);

			_workerThread = new Thread((object obj) =>
			{
				_startEvent.Set();
				ThreadWorker threadWorker = (ThreadWorker)obj;
				threadWorker.BeginWrite();
			});

			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}

			_workerThread.Start(_threadWorker);
		}

		public void WritePart(int partNumber, byte[] data)
		{
			_startEvent.Wait();
			_threadWorker.AddPartData(partNumber, data);
		}

		public void Dispose()
		{
			_cancellationTokenSource.Cancel();

			// Wait for ThreadWorker to finish the loop
			_cancellationEvent.Wait();
			if (!_threadWorker.EndedSuccessfully)
			{
				throw new ApplicationException("Data have not been written.");
			}

			_binaryWriter.Dispose();
			_fileStream.Dispose();

			File.Move(_tempFilePath, _filePath);
		}
	}
}

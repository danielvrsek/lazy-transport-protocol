using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace LazyTransportProtocol.Client.Services
{
	public delegate byte[] DownloadFilePart(int offset, int count);

	public delegate void ExceptionHandler(Exception e);

	public class ParallelFileDownloader
	{
		private readonly ConcurrentQueue<int> _partQueue;

		private readonly ConcurrentQueue<Thread> _workingThreads = new ConcurrentQueue<Thread>();

		private readonly object _syncLock = new object();

		public int Length { get; }

		public int PartLength { get; }

		public event ExceptionHandler ExceptionEvent;

		public event Action<int, byte[]> FilePartDownloadedEvent;

		public ParallelFileDownloader(int length, int partLength)
		{
			Length = length;
			PartLength = partLength;

			int numberOfParts = GetNumberOfParts(length, PartLength);
			var parts = Enumerable.Range(0, numberOfParts);

			_partQueue = new ConcurrentQueue<int>(parts);
		}

		private int GetNumberOfParts(int length, int partLength)
		{
			int numberOfParts = length / partLength;

			if (length % partLength > 0)
			{
				numberOfParts++;
			}

			return numberOfParts;
		}

		public void StartNew(DownloadFilePart downloadFilePart)
		{
			Thread workerThread = new Thread(() =>
			{
				Download(downloadFilePart);
				_workingThreads.TryDequeue(out Thread thread);
			});
			workerThread.Start();
			_workingThreads.Enqueue(workerThread);
		}

		public void Wait()
		{
			while (_workingThreads.Count > 0)
			{
				Thread.Sleep(1);
			}
		}

		private void Download(DownloadFilePart downloadFilePart)
		{
			while (_partQueue.TryDequeue(out int partNumber))
			{
				int offset = partNumber * PartLength;

				try
				{
					byte[] data = downloadFilePart(offset, PartLength);
					FilePartDownloadedEvent(partNumber, data);
				}
				catch (Exception e)
				{
					ExceptionEvent(e);
				}
			}
		}
	}
}
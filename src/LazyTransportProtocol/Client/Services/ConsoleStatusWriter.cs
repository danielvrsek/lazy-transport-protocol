using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LazyTransportProtocol.Client.Services
{
	public class ConsoleStatusWriter : IDisposable
	{
		private readonly object _lock = new object();
		private readonly string _label;
		private readonly long _totalSize;
		private long _downloaded = 0;
		private int _currentCursorPosition = 0;
		private int _cursorRow;

		private readonly Stopwatch _sw;
		private long _elapsed;

		private int StatusLength { get; set; } = 50;


		public ConsoleStatusWriter(string label, long totalSize)
		{
			_label = label + " ";
			_totalSize = totalSize;
			_cursorRow = Console.CursorTop;
			_sw = Stopwatch.StartNew();

			Console.CursorVisible = false;
			Write();
		}

		public void Update(int offset)
		{
			_downloaded += offset;

			Write();
		}

		private void Write()
		{
			double percentage = (double)_downloaded / _totalSize;
			int completed = (int)(percentage * StatusLength);

			string percentageString = $"{percentage.ToString("P").PadLeft(8)}";
			string status = "[" + "".PadLeft(completed, '%').PadRight(StatusLength, ' ') + "]";

			string text = _label + percentageString + status;
			_currentCursorPosition = 0;
			Write(text);
		}

		private void Write(string text)
		{
			lock (_lock)
			{
				Console.SetCursorPosition(_currentCursorPosition, _cursorRow);
				Console.Write(text);
				_elapsed = _sw.ElapsedMilliseconds;
			}
		}

		public void Dispose()
		{
			Console.CursorVisible = true;
			Console.WriteLine();
		}
	}
}

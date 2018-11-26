using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LazyTransportProtocol.Core.Application.Helpers
{
	public class AsynchronousQueue
	{
		private static event Action<object, Action> ItemAddedToQueue;

		static AsynchronousQueue()
		{
			ItemAddedToQueue += OnItemAddedToQueue;
		}

		public static void AddToQueue(object obj, Action action)
		{

			ItemAddedToQueue(obj, action);
		}

		private static void OnItemAddedToQueue(object obj, Action action)
		{
				lock (obj)
				{
					action();
					Task.Delay(500).Wait();
				}
		}
	}
}

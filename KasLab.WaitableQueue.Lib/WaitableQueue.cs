using System.Collections.Generic;
using System.Threading;

namespace KasLab.WaitableQueue.Lib
{
	public class WaitableQueue<TItem> : IWaitableQueue<TItem>
	{
		private readonly Queue<TItem> _queue;
		readonly Queue<AutoResetEvent> _waitQueue;
		private readonly object _lockObject = new object();

		public WaitableQueue()
		{
			_queue = new Queue<TItem>();
			_waitQueue = new Queue<AutoResetEvent>();

		}

		public void Push(TItem item)
		{
			lock (_lockObject)
			{
				_queue.Enqueue(item);
				if (_waitQueue.Count > 0)
					_waitQueue.Dequeue().Set();
			}
		}

		public TItem Pop()
		{
			if (_queue.Count == 0)
			{
				using (var wait = new AutoResetEvent(false))
				{
					lock (_lockObject)
					{
						_waitQueue.Enqueue(wait);
					}
					wait.WaitOne();
				}
			}
			lock (_lockObject)
			{
				return _queue.Dequeue();
			}
			
		}
	}
}


using System.Collections.Generic;
using System.Threading.Tasks;

namespace KasLab.WaitableQueue.Lib
{
	public class AsyncWaitableQueue<TItem> : IAsyncWaitableQueue<TItem>
	{
		private readonly Queue<TItem> _queue;
		readonly Queue<TaskCompletionSource<TItem>> _tasks;
		private readonly object _lockObject = new object();

		public AsyncWaitableQueue()
		{
			_queue = new Queue<TItem>();
			_tasks = new Queue<TaskCompletionSource<TItem>>();

		}

		public void Push(TItem item)
		{
			lock (_lockObject)
			{
				if (_tasks.Count > 0)
					_tasks.Dequeue().SetResult(item);
				else
					_queue.Enqueue(item);
			}
		}

		public Task<TItem> PopAsync()
		{
			lock (_lockObject)
			{
				if (_queue.Count == 0)
				{
					var tcs = new TaskCompletionSource<TItem>();
					_tasks.Enqueue(tcs);
					return tcs.Task;
				}
				else return Task.FromResult(_queue.Dequeue());
			}
		}
	}
}
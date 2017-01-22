using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using KasLab.WaitableQueue.Lib;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace KasLab.WaitableQueue.Tests
{
	[TestFixture(typeof(WaitableQueue<int>))]
    public class WaitableQueueTests<TQueue>
		where TQueue: IWaitableQueue<int>, new ()
    {
		[Test]
	    public async Task PushInMultiThread()
	    {
		    var q = new TQueue();
			var randomizer  = new Randomizer();
			var pushTasks = new Collection<Task>();
		    var tasksCount = 100;
			for (int i = 0; i < tasksCount; i++)
			{
				pushTasks.Add(Task.Run(() =>
				{
					q.Push(randomizer.Next());
				}));
			}
			Assert.AreEqual(tasksCount, pushTasks.Count);
		    await Task.WhenAll(pushTasks);
	    }
		

		[Test]
	    public async Task WaitForEmptyQueue()
		{
			var q = new TQueue();
			Task.Run(() => q.Pop())
				.ContinueWith(t =>
				{
					Assert.Fail("Not wait");
				});
			await Task.Delay(1000);
		}

		[Test]
	    public async Task PopInMultiThread()
	    {
			var q = new TQueue();
			var popTask = new Collection<Task>();
		    for (int i = 0; i < 10000; i++)
		    {
			    popTask.Add(Task.Run(() => q.Pop())
					.ContinueWith(t => Assert.Fail("Not wait")));
		    }
		    Task.WhenAll(popTask);
		    await Task.Delay(1000);
	    }

		[Test]
	    public async Task WaitPopWhilePushed()
	    {
		    var q = new TQueue();
			var task100 = Task.Run(() => q.Pop());
		    var task10 = Task.Run(() => q.Pop());
		    await Task.Delay(100);
			q.Push(100);
			q.Push(10);
		    await task100.ContinueWith(t => Assert.AreEqual(100, t.Result));
		    await task10.ContinueWith(t => Assert.AreEqual(10, t.Result));
	    }
    }
}

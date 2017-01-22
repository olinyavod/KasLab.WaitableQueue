using System.Threading.Tasks;

namespace KasLab.WaitableQueue.Lib
{
	public interface IAsyncWaitableQueue<TItem>
	{
		void Push(TItem item);

		Task<TItem> PopAsync();
	}
}
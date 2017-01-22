namespace KasLab.WaitableQueue.Lib
{
    public interface IWaitableQueue<TItem>
    {
	    void Push(TItem item);

	    TItem Pop();
    }
}

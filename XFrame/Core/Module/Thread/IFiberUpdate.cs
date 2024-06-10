
namespace XFrame.Core.Threads
{
    public interface IFiberUpdate
    {
        bool Disposed { get; }

        void OnUpdate(double escapeTime);
    }
}

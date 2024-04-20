
using XFrame.Tasks;

namespace XFrame.Core.Caches
{
    public interface ICacheObjectFactory : IProTaskHandler
    {
        ICacheObject Result { get; }

        void OnFactory();
        void OnFinish();
    }
}

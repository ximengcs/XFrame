
using XFrame.Modules.Tasks;

namespace XFrame.Core.Caches
{
    public interface ICacheObjectFactory : ITaskHandler
    {
        bool IsDone { get; }

        ICacheObject Result { get; }

        void OnFactory();
        void OnFinish();
    }
}

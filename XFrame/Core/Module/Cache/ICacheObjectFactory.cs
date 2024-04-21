using XFrame.Tasks;

namespace XFrame.Modules.Caches
{
    public interface ICacheObjectFactory : IProTaskHandler
    {
        ICacheObject Result { get; }

        void OnFactory();
        void OnFinish();
    }
}

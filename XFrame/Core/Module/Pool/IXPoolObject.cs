
namespace XFrame.Modules.Pools
{
    public interface IXPoolObject : IPoolObject
    {
        new IPool InPool { get; set; }
        void OnCreateFromPool();
        void OnRequestFromPool();
        void OnReleaseFromPool();
        void OnDestroyFromPool();
    }
}

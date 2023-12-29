
namespace XFrame.Modules.Pools
{
    public abstract class PoolObjectBase : IXPoolObject
    {
        public string MarkName { get; set; }

        public IPool InPool { get; set; }

        public int PoolKey { get; protected set; }

        void IXPoolObject.OnCreateFromPool() { OnCreateFromPool(); }
        protected virtual void OnCreateFromPool() { }

        void IXPoolObject.OnRequestFromPool() { OnRequestFromPool(); }
        protected virtual void OnRequestFromPool() { }

        void IXPoolObject.OnDestroyFromPool() { OnDestroyFromPool(); }
        protected virtual void OnDestroyFromPool() { }

        void IXPoolObject.OnReleaseFromPool() { OnReleaseFromPool(); }
        protected virtual void OnReleaseFromPool() { }

    }
}

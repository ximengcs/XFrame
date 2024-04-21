
namespace XFrame.Modules.Pools
{
    public abstract class PoolObjectBase : IPoolObject
    {
        protected IPoolModule m_Module;

        public string MarkName { get; set; }    

        public int PoolKey { get; protected set; }

        IPool IPoolObject.InPool { get; set; }

        void IPoolObject.OnCreate(IPoolModule module)
        {
            m_Module = module;
            OnCreateFromPool();
        }

        void IPoolObject.OnDelete()
        {
            OnDestroyFromPool();
        }

        void IPoolObject.OnRelease()
        {
            OnReleaseFromPool();
        }

        void IPoolObject.OnRequest()
        {
            OnRequestFromPool();
        }

        protected internal virtual void OnCreateFromPool() { }
        protected internal virtual void OnRequestFromPool() { }
        protected internal virtual void OnDestroyFromPool() { }
        protected internal virtual void OnReleaseFromPool() { }
    }
}

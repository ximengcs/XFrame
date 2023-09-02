using System;
using XFrame.Modules.Tasks;

namespace XFrame.Core.Caches
{
    public partial class CacheObjectTask : TaskBase
    {
        private Action<ICacheObject> m_Callback;
        public ICacheObject CacheObject { get; private set; }

        protected override void OnInit()
        {
            AddStrategy(new Strategy());
        }

        public CacheObjectTask OnComplete(Action<ICacheObject> callback)
        {
            m_Callback = callback;
            return this;
        }

        protected override void InnerComplete()
        {
            base.InnerComplete();
            m_Callback?.Invoke(CacheObject);
            m_Callback = null;
        }

        protected override void OnReleaseFromPool()
        {
            base.OnReleaseFromPool();
            CacheObject = null;
            m_Callback = null;
        }
    }
}

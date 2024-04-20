using System;
using XFrame.Tasks;

namespace XFrame.Core.Caches
{
    public partial class CacheObjectTask : XProTask<ICacheObject>
    {
        private ICacheObjectFactory m_Handler;
        private Action<ICacheObject> m_Callback;

        public ICacheObject CacheObject => m_Handler.Result;

        public CacheObjectTask(ICacheObjectFactory factory) : base(factory)
        {
            m_Handler = factory;
            m_Handler.OnFactory();
        }

        public override ICacheObject GetResult()
        {
            return CacheObject;
        }

        public CacheObjectTask OnCompleted(Action<ICacheObject> callback)
        {
            m_Callback = callback;
            return this;
        }

        protected override void InnerExecComplete()
        {
            if (m_Callback != null)
            {
                if (m_Handler.Result != null)
                    XCache.Event.Trigger(CacheObjectFactoryEvent.Create(m_Handler.Result.GetType()));
                m_Handler.OnFinish();
                m_Handler = null;
                m_Callback?.Invoke(CacheObject);
                m_Callback = null;
            }
            base.InnerExecComplete();
        }
    }
}

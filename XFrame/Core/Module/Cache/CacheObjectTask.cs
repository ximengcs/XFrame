using System;
using XFrame.Tasks;

namespace XFrame.Modules.Caches
{
    public partial class CacheObjectTask : XProTask<ICacheObject>
    {
        private ICacheObjectFactory m_Handler;

        public ICacheObject CacheObject => m_Handler.Result;

        public CacheObjectTask(ICacheObjectFactory factory) : base(factory)
        {
            m_Handler = factory;
        }

        public override ICacheObject GetResult()
        {
            return CacheObject;
        }

        protected override void InnerStart()
        {
            base.InnerStart();
            m_Handler.OnFactory();
        }

        protected override void InnerExecComplete()
        {
            m_Handler.OnFinish();
            m_Handler = null;

            base.InnerExecComplete();
        }
    }
}

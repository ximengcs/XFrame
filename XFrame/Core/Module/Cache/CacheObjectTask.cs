using XFrame.Tasks;

namespace XFrame.Modules.Caches
{
    internal partial class CacheObjectTask : XProTask<ICacheObject>
    {
        private ICacheObjectFactory m_Handler;
        private ICacheObject m_Object;

        public ICacheObject CacheObject => m_Object;

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
            m_Object = m_Handler.Result;
            m_Handler.OnFinish();
            m_Handler = null;
            base.InnerExecComplete();
        }
    }
}

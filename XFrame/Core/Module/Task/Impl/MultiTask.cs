using XFrame.Modules.Pools;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;

namespace XFrame.Modules.Tasks
{
    public partial class MultiTask : TaskBase
    {
        private Handler m_UseHandler;

        public List<ITask> Tasks => m_UseHandler.Tasks;

        protected internal override void OnCreateFromPool()
        {
            base.OnCreateFromPool();
            AddStrategy(new Strategy());
        }

        public void MarkReady()
        {
            m_UseHandler.CanContinue = false;
        }

        public void MarkStart()
        {
            m_UseHandler.CanContinue = true;
        }

        public override ITask Add(ITaskHandler data)
        {
            ITask task = data as ITask;
            if (task == null)
            {
                Log.Debug("XFrame", "MultiTask only add ITask");
            }
            else
            {
                if (m_UseHandler == null)
                {
                    m_UseHandler = References.Require<Handler>();
                    base.Add(m_UseHandler);
                }
                m_UseHandler.Add(task);
            }

            return this;
        }

        protected internal override void OnReleaseFromPool()
        {
            base.OnReleaseFromPool();
            References.Release(m_UseHandler);
            m_UseHandler = null;
        }
    }
}

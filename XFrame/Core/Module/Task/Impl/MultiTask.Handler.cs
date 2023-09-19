using System.Collections.Generic;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Tasks
{
    public partial class MultiTask
    {
        private class Handler : PoolObjectBase, ITaskHandler, IPoolObject
        {
            private int m_LastCheck;
            private List<ITask> m_Tasks;

            public List<ITask> Tasks => m_Tasks;

            public bool CanContinue { get; set; }

            public int Count => m_Tasks.Count;

            public void Add(ITask task)
            {
                m_Tasks.Add(task);
            }

            public int Check()
            {
                if (!CanContinue)
                    return m_LastCheck;

                int finishCount = 0;
                foreach (ITask task in m_Tasks)
                {
                    if (task.IsComplete)
                        finishCount++;
                }
                m_LastCheck = finishCount;
                return finishCount;
            }

            protected internal override void OnCreateFromPool()
            {
                base.OnCreateFromPool();
                m_Tasks = new List<ITask>();
            }

            protected internal override void OnRequestFromPool()
            {
                base.OnRequestFromPool();
                m_LastCheck = 0;
                CanContinue = true;
            }

            protected internal override void OnReleaseFromPool()
            {
                base.OnReleaseFromPool();
                m_Tasks.Clear();
            }
        }
    }
}

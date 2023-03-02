using System;
using XFrame.Core;
using System.Threading;
using System.Collections.Generic;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Threads
{
    /// <summary>
    /// 主线程上下文处理
    /// </summary>
    public class MainSynchronizationContext : SynchronizationContext, IModule
    {
        private int m_MainThread;
        private Queue<Action> m_ActQueue;

        public int Id => default;

        void IModule.OnInit(object data)
        {
            m_MainThread = Thread.CurrentThread.ManagedThreadId;
            m_ActQueue = new Queue<Action>();
            SetSynchronizationContext(this);
        }

        void IModule.OnUpdate(float escapeTime)
        {
            if (m_ActQueue.Count <= 0)
                return;

            if (m_MainThread == Thread.CurrentThread.ManagedThreadId)
                m_ActQueue.Dequeue()();
        }

        void IModule.OnDestroy()
        {

        }

        public override void Post(SendOrPostCallback d, object state)
        {
            m_ActQueue.Enqueue(() => d(state));
        }
    }
}

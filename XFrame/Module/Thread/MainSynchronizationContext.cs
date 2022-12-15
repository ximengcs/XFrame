using System;
using XFrame.Core;
using System.Threading;
using System.Collections.Generic;

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

        public void OnInit(object data)
        {
            m_MainThread = Thread.CurrentThread.ManagedThreadId;
            m_ActQueue = new Queue<Action>();
            SetSynchronizationContext(this);
        }

        public void OnUpdate(float escapeTime)
        {
            if (m_ActQueue.Count <= 0)
                return;

            if (m_MainThread == Thread.CurrentThread.ManagedThreadId)
                m_ActQueue.Dequeue()();
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            m_ActQueue.Enqueue(() => d(state));
        }

        public void OnDestroyFrom()
        {
            m_ActQueue.Clear();
            m_ActQueue = null;
        }

        public void OnCreate()
        {

        }

        public void OnRelease()
        {

        }

        public void OnDestroy()
        {

        }
    }
}

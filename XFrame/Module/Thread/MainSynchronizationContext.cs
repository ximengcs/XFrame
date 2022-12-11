using System;
using System.Collections.Generic;
using System.Threading;
using XFrame.Core;

namespace XFrame.Modules
{
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

        public void OnDestroy()
        {
            m_ActQueue.Clear();
            m_ActQueue = null;
        }

        public void OnCreate(IPool from)
        {

        }

        public void OnRelease(IPool from)
        {

        }

        public void OnDestroy(IPool from)
        {

        }
    }
}

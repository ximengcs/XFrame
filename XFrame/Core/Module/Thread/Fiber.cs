using System;
using System.Collections.Generic;
using System.Threading;
using XFrame.Modules.Diagnotics;

namespace XFrame.Core.Threads
{
    public class Fiber
    {
        private int m_Type;
        private Thread m_Thread;
        private List<IFiberUpdate> m_UpdaterList;
        private List<IFiberUpdate> m_CacheList;
        private FiberSynchronizationContext m_Context;
        private bool m_Disposed;

        internal int Thread { get { return m_Context.ThreadId; } }

        public int Type => m_Type;

        public bool Disposed => m_Disposed;

        public Fiber(int type, int threadId = -1)
        {
            m_Type = type;
            m_CacheList = new List<IFiberUpdate>();
            m_UpdaterList = new List<IFiberUpdate>();
            m_Context = new FiberSynchronizationContext(threadId);
        }

        public void SetThread(Thread thread)
        {
            m_Thread = thread;
            Log.Debug(Log.Fiber, $"create fiber {m_Type} {Thread}");
        }

        public void Dispose()
        {
            m_Disposed = true;
            try
            {
                m_Thread.Interrupt();
                m_Thread.Abort();
            }
            catch (Exception e)
            {
                Log.Exception(e);
            }

            m_Thread = null;
        }

        public bool CheckContent(SynchronizationContext context)
        {
            return m_Context == context;
        }

        public Fiber Use()
        {
            if (SynchronizationContext.Current != m_Context)
            {
                SynchronizationContext current = SynchronizationContext.Current;
                SynchronizationContext.SetSynchronizationContext(m_Context);
                return Entry.GetModule<FiberModule>().FindFiber(current);
            }
            else
            {
                return this;
            }
        }

        public void Post(SendOrPostCallback handler, object state)
        {
            m_Context.Post(handler, state);
        }

        public void Update(double escapeTime)
        {
            m_CacheList.Clear();
            m_CacheList.AddRange(m_UpdaterList);
            foreach (IFiberUpdate updater in m_CacheList)
                updater.OnUpdate(escapeTime);
            m_Context.OnUpdate(escapeTime);
        }

        public void RegisterUpdater(IFiberUpdate updater)
        {
            m_UpdaterList.Add(updater);
        }

        public void UnRegisterUpdater(IFiberUpdate updater)
        {
            m_UpdaterList.Remove(updater);
        }
    }

}

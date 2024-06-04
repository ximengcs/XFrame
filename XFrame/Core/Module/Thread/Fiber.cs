using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;
using XFrame.Modules.Diagnotics;

namespace XFrame.Core.Threads
{
    public class Fiber
    {
        private int m_Type;
        private List<IFiberUpdate> m_UpdaterList;
        private List<IFiberUpdate> m_CacheList;
        private FiberSynchronizationContext m_Context;
        private bool m_Disposed;
        private string m_Name;

        internal int Thread => m_Context.ThreadId;

        private bool IsMain => m_Type == 0;

        public int Type => m_Type;

        public bool Disposed => m_Disposed;

        public Fiber(string name, int type)
        {
            m_Type = type;
            m_Name = name;
            m_CacheList = new List<IFiberUpdate>();
            m_UpdaterList = new List<IFiberUpdate>();
            m_Context = new FiberSynchronizationContext();
        }

        public void SetThread(int thread)
        {
            m_Context.SetThread(thread);
            Log.Debug(Log.Fiber, $"create fiber [{m_Name}] {m_Type} {Thread}");
        }

        public void Dispose()
        {
            if (IsMain)
                return;
            Log.Debug(Log.Fiber, $"dispose fiber [{m_Name}] {m_Type} {Thread}");
            m_Disposed = true;
            m_Context.OnDestroy();
            m_CacheList = null;
            m_UpdaterList = null;
            m_Context = null;
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
            if (m_Disposed)
                return;
            m_Context.Post(handler, state);
        }

        public void Update(double escapeTime)
        {
            if (m_Disposed)
                return;
            m_CacheList.Clear();
            m_CacheList.AddRange(m_UpdaterList);
            foreach (IFiberUpdate updater in m_CacheList)
            {
                if (!updater.Disposed)
                    updater.OnUpdate(escapeTime);
            }
            m_Context.OnUpdate(escapeTime);
        }

        public void RegisterUpdater(IFiberUpdate updater)
        {
            if (m_Disposed)
                return;
            m_UpdaterList.Add(updater);
        }

        public void UnRegisterUpdater(IFiberUpdate updater)
        {
            if (m_Disposed)
                return;
            m_UpdaterList.Remove(updater);
        }
    }

}

using System;
using System.Collections.Concurrent;
using System.Threading;
using XFrame.Tasks;

namespace XFrame.Core.Threads
{
    public class FiberSynchronizationContext : SynchronizationContext
    {
        #region Inner Fields
        private int m_ThreadId;
        private ConcurrentQueue<Pair<SendOrPostCallback, object>> m_ActQueue;
        private const long DEFAULT_TIMEOUT = -1;
        #endregion

        #region Interface
        /// <summary>
        /// 最大超时(毫秒)
        /// </summary>
        public long ExecTimeout { get; set; }

        #endregion

        #region IModule Life Fun
        /// <inheritdoc/>
        public int Id => default;

        /// <inheritdoc/>
        public XDomain Domain => XTaskHelper.Domain;

        internal int ThreadId => m_ThreadId;

        public FiberSynchronizationContext()
        {
            m_ActQueue = new ConcurrentQueue<Pair<SendOrPostCallback, object>>();
            ExecTimeout = DEFAULT_TIMEOUT;
        }

        internal void SetThread(int thread)
        {
            m_ThreadId = thread;
        }

        public void OnDestroy()
        {
            m_ActQueue = null;
            m_ThreadId = default;
            ExecTimeout = default;
        }

        public void OnUpdate(double escapeTime)
        {
            if (m_ThreadId != Thread.CurrentThread.ManagedThreadId)
                return;

            if (m_ActQueue.Count <= 0)
                return;

            long timeout = 0;
            long now = DateTime.Now.Ticks;
            while (m_ActQueue.Count > 0)
            {
                if (m_ActQueue.TryDequeue(out Pair<SendOrPostCallback, object> item))
                {
                    item.Key(item.Value);
                }
                long escape = DateTime.Now.Ticks - now;
                timeout += escape / TimeSpan.TicksPerMillisecond;
                if (ExecTimeout != -1 && timeout >= ExecTimeout)
                    break;
            }
        }
        #endregion

        #region System Life Fun

        /// <inheritdoc/>
        public override void Post(SendOrPostCallback d, object state)
        {
            m_ActQueue.Enqueue(Pair.Create(d, state));
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            Post(d, state);
        }
        #endregion
    }
}

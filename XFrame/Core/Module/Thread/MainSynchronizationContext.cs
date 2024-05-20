using XFrame.Core;
using XFrame.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Collections.Concurrent;
using System;

namespace XFrame.Modules.Threads
{
    /// <summary>
    /// 主线程上下文处理
    /// </summary>
    public class MainSynchronizationContext : SynchronizationContext, IModule, IUpdater, IFinishUpdater
    {
        #region Inner Fields

        private int m_MainThread;
        private ConcurrentQueue<Pair<SendOrPostCallback, object>> m_ActQueue;
        private ConcurrentQueue<Pair<SendOrPostCallback, object>> m_UpdateAfterActQueue;
        private const long DEFAULT_TIMEOUT = 10;

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

        void IModule.OnInit(object data, ModuleConfigAction configCallback)
        {
            m_MainThread = Thread.CurrentThread.ManagedThreadId;
            m_ActQueue = new ConcurrentQueue<Pair<SendOrPostCallback, object>>();
            m_UpdateAfterActQueue = new ConcurrentQueue<Pair<SendOrPostCallback, object>>();
            ExecTimeout = DEFAULT_TIMEOUT;
            SetSynchronizationContext(this);
        }

        void IModule.OnStart()
        {
        }

        void IUpdater.OnUpdate(double escapeTime)
        {
            if (m_ActQueue.Count <= 0)
                return;

            if (m_MainThread == Thread.CurrentThread.ManagedThreadId)
            {
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
        }

        void IFinishUpdater.OnUpdate(double escapeTime)
        {
            if (m_UpdateAfterActQueue.Count <= 0)
                return;

            if (m_MainThread == Thread.CurrentThread.ManagedThreadId)
            {
                long timeout = 0;
                while (m_UpdateAfterActQueue.Count > 0)
                {
                    long now = DateTime.Now.Ticks;
                    if (m_UpdateAfterActQueue.TryDequeue(out Pair<SendOrPostCallback, object> item))
                    {
                        item.Key(item.Value);
                    }
                    long escape = DateTime.Now.Ticks - now;
                    timeout += escape / TimeSpan.TicksPerMillisecond;
                    if (ExecTimeout != -1 && timeout >= ExecTimeout)
                        break;
                }
            }
        }

        void IModule.OnDestroy()
        {
        }

        #endregion

        #region System Life Fun

        /// <inheritdoc/>
        public override void Post(SendOrPostCallback d, object state)
        {
            m_ActQueue.Enqueue(Pair.Create(d, state));
        }

        public void PostFinishUpdate(SendOrPostCallback d, object state)
        {
            m_UpdateAfterActQueue.Enqueue(Pair.Create(d, state));
        }
        #endregion
    }
}
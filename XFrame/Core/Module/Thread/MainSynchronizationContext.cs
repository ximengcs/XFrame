using System;
using XFrame.Core;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using XFrame.Tasks;

namespace XFrame.Modules.Threads
{
    /// <summary>
    /// 主线程上下文处理
    /// </summary>
    public class MainSynchronizationContext : SynchronizationContext, IModule, IUpdater
    {
        #region Inner Fields

        private int m_MainThread;
        private Queue<Pair<SendOrPostCallback, object>> m_ActQueue;
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
            m_ActQueue = new Queue<Pair<SendOrPostCallback, object>>();
            ExecTimeout = DEFAULT_TIMEOUT;
            SetSynchronizationContext(this);
        }

        void IModule.OnStart()
        {
        }

        void IUpdater.OnUpdate(float escapeTime)
        {
            if (m_ActQueue.Count <= 0)
                return;

            if (m_MainThread == Thread.CurrentThread.ManagedThreadId)
            {
                long timeout = 0;
                Stopwatch sw = new Stopwatch();
                while (m_ActQueue.Count > 0)
                {
                    sw.Restart();
                    Pair<SendOrPostCallback, object> item = m_ActQueue.Dequeue();
                    item.Key(item.Value);
                    sw.Stop();
                    timeout += sw.ElapsedMilliseconds;
                    if (timeout >= ExecTimeout)
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

        #endregion
    }
}
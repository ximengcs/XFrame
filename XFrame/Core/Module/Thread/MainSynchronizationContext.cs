using XFrame.Core;
using System.Threading;
using System.Diagnostics;
using XFrame.Tasks;
using System.Collections.Concurrent;
using XFrame.Modules.Diagnotics;
using System;

namespace XFrame.Modules.Threads
{
    /// <summary>
    /// 主线程上下文处理
    /// </summary>
    public class MainSynchronizationContext : SynchronizationContext, IModule, IUpdater
    {
        #region Inner Fields

        private int m_MainThread;
        private ConcurrentQueue<Pair<SendOrPostCallback, object>> m_ActQueue;
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

            Log.Debug($" thread update 1, escape {escapeTime}, {m_MainThread} {Thread.CurrentThread.ManagedThreadId} {new TimeSpan(DateTime.Now.Ticks).TotalMilliseconds} ");
            if (m_MainThread == Thread.CurrentThread.ManagedThreadId)
            {
                long timeout = 0;
                Stopwatch sw = new Stopwatch();
                while (m_ActQueue.Count > 0)
                {
                    sw.Restart();
                    if (m_ActQueue.TryDequeue(out Pair<SendOrPostCallback, object> item))
                    {
                        item.Key(item.Value);
                    }
                    sw.Stop();
                    timeout += sw.ElapsedMilliseconds;
                    if (ExecTimeout != -1 && timeout >= ExecTimeout)
                        break;
                }
            }
            Log.Debug($" thread update 2, escape {escapeTime}, {m_MainThread} {Thread.CurrentThread.ManagedThreadId} {new TimeSpan(DateTime.Now.Ticks).TotalMilliseconds} ");
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
using System;
using XFrame.Core;
using System.Threading;
using System.Diagnostics;
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
        private const long DEFAULT_TIMEOUT = 10;

        /// <summary>
        /// 最大超时(毫秒)
        /// </summary>
        public long ExecTimeout { get; set; }

        public int Id => default;

        void IModule.OnInit(object data)
        {
            m_MainThread = Thread.CurrentThread.ManagedThreadId;
            m_ActQueue = new Queue<Action>();
            ExecTimeout = DEFAULT_TIMEOUT;
            SetSynchronizationContext(this);
        }

        void IModule.OnStart()
        {

        }

        void IModule.OnUpdate(float escapeTime)
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
                    m_ActQueue.Dequeue()();
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

        public override void Post(SendOrPostCallback d, object state)
        {
            m_ActQueue.Enqueue(() => d(state));
        }
    }
}

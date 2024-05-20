using XFrame.Core;
using System.Diagnostics;
using XFrame.Modules.Pools;
using XFrame.Modules.Times;
using XFrame.Modules.Rand;
using System.Collections.Generic;
using XFrame.Collections;

namespace XFrame.Modules.Tasks
{
    /// <inheritdoc/>
    [BaseModule]
    [RequireModule(typeof(RandModule))]
    [RequireModule(typeof(PoolModule))]
    [RequireModule(typeof(TimeModule))]
    [XType(typeof(ITaskModule))]
    public class TaskModule : ModuleBase, ITaskModule
    {
        #region Const Fields
        private const long DEFAULT_TIMEOUT = 10;
        #endregion

        #region Inner Fields
        private List<IUpdater> m_Tasks;
        private Stopwatch m_Watch;
        private float m_ThisFrameTime;
        #endregion

        #region Life Fun
        /// <inheritdoc/>
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            TaskTimeout = DEFAULT_TIMEOUT;
            m_Tasks = new List<IUpdater>(32);
            m_Watch = new Stopwatch();
        }

        /// <inheritdoc/>
        public void OnUpdate(double escapeTime)
        {
            m_ThisFrameTime = 0;
            for (int i = m_Tasks.Count - 1; i >= 0; i--)
            {
                IUpdater task = m_Tasks[i];
                InnerExecTask(task, escapeTime);
                if (!InnerCanContinue())
                    break;
            }
        }
        #endregion

        internal void InnerExecTask(IUpdater task, double escapeTime)
        {
            m_Watch.Restart();
            task.OnUpdate(escapeTime);
            m_Watch.Stop();
            m_ThisFrameTime += m_Watch.ElapsedMilliseconds;
        }

        internal bool InnerCanContinue()
        {
            return m_ThisFrameTime < TaskTimeout;
        }

        #region Interface
        /// <inheritdoc/>
        public int ExecCount => m_Tasks.Count;

        /// <inheritdoc/>
        public long TaskTimeout { get; set; }

        /// <inheritdoc/>
        public void Register(IUpdater task)
        {
            m_Tasks.Add(task);
        }

        /// <inheritdoc/>
        public void UnRegister(IUpdater task)
        {
            m_Tasks.Remove(task);
        }
        #endregion
    }
}

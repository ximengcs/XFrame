using XFrame.Core;
using XFrame.Module.Rand;
using System.Diagnostics;
using XFrame.Modules.Pools;
using XFrame.Modules.Times;
using System.Collections.Generic;
using XFrame.Modules.Diagnotics;

namespace XFrame.Modules.Tasks
{
    /// <summary>
    /// 任务模块
    /// </summary>
    [BaseModule]
    [RequireModule(typeof(RandModule))]
    [RequireModule(typeof(PoolModule))]
    [RequireModule(typeof(TimeModule))]
    public class TaskModule : SingletonModule<TaskModule>
    {
        #region Const Fields
        private const long DEFAULT_TIMEOUT = 10;
        #endregion

        #region Inner Fields
        private List<ITask> m_Tasks;
        private Stopwatch m_Watch;
        private float m_ThisFrameTime;
        private Dictionary<string, ITask> m_TaskWithName;
        #endregion

        #region Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            TaskTimeout = DEFAULT_TIMEOUT;
            m_Tasks = new List<ITask>();
            m_Watch = new Stopwatch();
            m_TaskWithName = new Dictionary<string, ITask>();
        }

        protected override void OnUpdate(float escapeTime)
        {
            base.OnUpdate(escapeTime);

            m_ThisFrameTime = 0;
            for (int i = m_Tasks.Count - 1; i >= 0; i--)
            {
                ITask task = m_Tasks[i];
                if (task.IsStart)
                {
                    InnerExecTask(task);
                    if (task.IsComplete)
                    {
                        m_Tasks.RemoveAt(i);
                        m_TaskWithName.Remove(task.Name);
                    }
                    if (!InnerCanContinue())
                        break;
                }
            }
        }
        #endregion

        internal void InnerExecTask(ITask task)
        {
            m_Watch.Restart();
            task.OnUpdate();
            m_Watch.Stop();
            m_ThisFrameTime += m_Watch.ElapsedMilliseconds;
        }

        internal bool InnerCanContinue()
        {
            return m_ThisFrameTime < TaskTimeout;
        }

        #region Interface
        /// <summary>
        /// 任务模块最大超时
        /// </summary>
        public long TaskTimeout { get; set; }

        /// <summary>
        /// 获取(不存在时创建)一个任务
        /// </summary>
        /// <typeparam name="T">任务类型</typeparam>
        /// <returns>获取到的任务</returns>
        public T GetOrNew<T>() where T : ITask
        {
            return GetOrNew<T>(RandModule.Inst.RandString());
        }

        /// <summary>
        /// 获取一个任务
        /// </summary>
        /// <typeparam name="T">任务类型</typeparam>
        /// <returns>任务实例</returns>
        public T Get<T>(string name) where T : ITask
        {
            if (m_TaskWithName.TryGetValue(name, out ITask task))
                return (T)task;
            return default;
        }

        public ITask Get(string name)
        {
            if (m_TaskWithName.TryGetValue(name, out ITask task))
                return task;
            return default;
        }

        /// <summary>
        /// 获取(不存在时创建)一个任务
        /// </summary>
        /// <typeparam name="T">任务类型</typeparam>
        /// <param name="name">任务名</param>
        /// <returns>获取到的任务</returns>
        public T GetOrNew<T>(string name) where T : ITask
        {
            if (!m_TaskWithName.TryGetValue(name, out ITask task))
            {
                IPool<T> pool = PoolModule.Inst.GetOrNew<T>();
                IPoolObject obj = pool.Require();
                task = (ITask)obj;
                m_TaskWithName[name] = task;
                m_Tasks.Add(task);
                task.OnInit(name);
            }
            return (T)task;
        }

        /// <summary>
        /// 移除一个任务
        /// </summary>
        /// <param name="name">任务名</param>
        public void Remove(string name)
        {
            if (m_TaskWithName.ContainsKey(name))
            {
                m_TaskWithName.Remove(name);
                for (int i = 0; i < m_Tasks.Count; i++)
                {
                    ITask task = m_Tasks[i];
                    if (task.Name == name)
                    {
                        m_Tasks.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        #endregion
    }
}

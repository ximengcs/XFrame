using XFrame.Core;
using System.Diagnostics;
using XFrame.Modules.Pools;
using XFrame.Modules.Times;
using XFrame.Modules.Rand;
using System.Collections.Generic;
using XFrame.Collections;

namespace XFrame.Modules.Tasks
{
    /// <summary>
    /// 任务模块
    /// </summary>
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
            m_Tasks = new List<ITask>(32);
            m_Watch = new Stopwatch();
            m_TaskWithName = new Dictionary<string, ITask>();
        }

        public void OnUpdate(float escapeTime)
        {
            m_ThisFrameTime = 0;
            for (int i = m_Tasks.Count - 1; i >= 0; i--)
            {
                ITask task = m_Tasks[i];
                if (task.IsStart)
                {
                    InnerExecTask(task);
                    if (task.IsComplete)
                    {
                        if (m_TaskWithName.ContainsKey(task.Name))
                        {
                            m_Tasks.RemoveAt(i);
                            m_TaskWithName.Remove(task.Name);
                        }
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
        public int ExecCount => m_Tasks.Count;

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
            return GetOrNew<T>(XModule.Rand.RandString());
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
                IPool<T> pool = XModule.Pool.GetOrNew<T>();
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
        /// <param name="task">任务</param>
        public bool Remove(ITask task)
        {
            bool success = Remove(task.Name);
            if (!success)
                References.Release(task);
            return success;
        }

        /// <summary>
        /// 移除一个任务
        /// </summary>
        /// <param name="name">任务名</param>
        public bool Remove(string name)
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
                        References.Release(task);
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion
    }
}

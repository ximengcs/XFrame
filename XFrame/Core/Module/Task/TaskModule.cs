using System;
using XFrame.Core;
using XFrame.Module.Rand;
using XFrame.Modules.Pools;
using System.Collections.Generic;
using XFrame.Modules.Times;
using System.Diagnostics;

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
        private List<ITask> m_Tasks;
        private Dictionary<string, ITask> m_TaskWithName;
        private const long DEFAULT_TIMEOUT = 10;

        public long TaskTimeout { get; set; }

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            TaskTimeout = DEFAULT_TIMEOUT;
            m_Tasks = new List<ITask>();
            m_TaskWithName = new Dictionary<string, ITask>();
        }

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
                task = Activator.CreateInstance<T>();
                m_TaskWithName[name] = task;
                m_Tasks.Add(task);
                task.OnInit(name);
            }
            return (T)task;
        }

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

        protected override void OnUpdate(float escapeTime)
        {
            base.OnUpdate(escapeTime);

            long timeout = 0;
            Stopwatch sw = new Stopwatch();
            for (int i = m_Tasks.Count - 1; i >= 0; i--)
            {
                sw.Restart();
                ITask task = m_Tasks[i];
                if (task.IsStart)
                    task.OnUpdate();

                if (task.IsComplete)
                {
                    m_Tasks.RemoveAt(i);
                    m_TaskWithName.Remove(task.Name);
                }
                sw.Stop();
                timeout += sw.ElapsedMilliseconds;
                if (timeout >= TaskTimeout)
                    break;
            }
        }
    }
}

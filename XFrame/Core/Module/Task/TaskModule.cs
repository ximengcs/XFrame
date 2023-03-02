using System;
using XFrame.Core;
using System.Collections.Generic;
using XFrame.Module.Rand;

namespace XFrame.Modules.Tasks
{
    /// <summary>
    /// 任务模块
    /// </summary>
    public class TaskModule : SingletonModule<TaskModule>
    {
        private List<ITask> m_Tasks;
        private Dictionary<string, ITask> m_TaskWithName;
        private List<string> m_WillDel;

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Tasks = new List<ITask>();
            m_WillDel = new List<string>();
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
                task.OnInit();
            }
            return (T)task;
        }

        protected override void OnUpdate(float escapeTime)
        {
            base.OnUpdate(escapeTime);

            for (int i = m_Tasks.Count - 1; i >= 0; i--)
            {
                ITask task = m_Tasks[i];
                if (task.IsStart)
                {
                    task.OnUpdate();
                    if (task.IsComplete)
                        m_Tasks.RemoveAt(i);
                }
            }

            m_WillDel.Clear();
            foreach (var item in m_TaskWithName)
            {
                ITask task = item.Value;
                if (task.IsStart)
                {
                    task.OnUpdate();
                    if (task.IsComplete)
                        m_WillDel.Add(item.Key);
                }
            }
            foreach (string name in m_WillDel)
                m_TaskWithName.Remove(name);
        }
    }
}

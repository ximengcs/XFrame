using System;
using System.Collections.Generic;
using XFrame.Core;

namespace XFrame.Modules
{
    public class TaskModule : SingletonModule<TaskModule>
    {
        private List<ITask> m_Tasks;
        private Dictionary<string, ITask> m_TaskWithName;
        private List<string> m_WillDel;

        public override void OnInit(object data)
        {
            base.OnInit(data);
            m_Tasks = new List<ITask>();
            m_WillDel = new List<string>();
            m_TaskWithName = new Dictionary<string, ITask>();
        }

        public T New<T>() where T : ITask
        {
            T task = Activator.CreateInstance<T>();
            task.OnInit();
            m_Tasks.Add(task);
            return task;
        }

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

        public override void OnUpdate(float escapeTime)
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

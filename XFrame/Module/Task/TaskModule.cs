using System.Collections.Generic;
using XFrame.Core;

namespace XFrame.Modules
{
    public class TaskModule : SingleModule<TaskModule>
    {
        private List<XTask> m_Tasks;
        private Dictionary<string, XTask> m_TaskWithName;
        private List<string> m_WillDel;

        public override void OnInit(object data)
        {
            base.OnInit(data);
            m_Tasks = new List<XTask>();
            m_WillDel = new List<string>();
            m_TaskWithName = new Dictionary<string, XTask>();
        }

        public XTask New()
        {
            XTask task = new XTask();
            m_Tasks.Add(task);
            return task;
        }

        public XTask GetOrNew(string name)
        {
            if (!m_TaskWithName.TryGetValue(name, out XTask task))
            {
                task = new XTask();
                m_TaskWithName[name] = task;
            }
            return task;
        }

        public override void OnUpdate(float escapeTime)
        {
            base.OnUpdate(escapeTime);

            for (int i = m_Tasks.Count - 1; i >= 0; i--)
            {
                XTask task = m_Tasks[i];
                if (task.IsStart)
                {
                    task.Update();
                    if (task.IsComplete)
                        m_Tasks.RemoveAt(i);
                }
            }

            m_WillDel.Clear();
            foreach (var item in m_TaskWithName)
            {
                XTask task = item.Value;
                if (task.IsStart)
                {
                    task.Update();
                    if (task.IsComplete)
                        m_WillDel.Add(item.Key);
                }
            }
            foreach (string name in m_WillDel)
                m_TaskWithName.Remove(name);
        }
    }
}

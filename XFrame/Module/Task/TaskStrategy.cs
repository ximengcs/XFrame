using System;

namespace XFrame.Modules.Tasks
{
    /// <summary>
    /// 通用任务处理策略
    /// </summary>
    public class TaskStrategy : ITaskStrategy
    {
        private float m_Pro;
        private Type m_Type = typeof(ITask);
        public Type HandleType => m_Type;

        public float Handle(ITask from, ITaskHandler target)
        {
            ITask task = (ITask)target;
            if (task.IsStart)
            {
                task.OnUpdate();
                if (task.IsComplete)
                {
                    m_Pro = TaskBase.MAX_PRO;
                }
                else
                {
                    m_Pro += 0.1f;
                    m_Pro = Math.Min(m_Pro, 0.9f);
                }
            }

            return m_Pro;
        }

        public void Use()
        {
            m_Pro = 0;
        }
    }
}

using System;

namespace XFrame.Modules.Tasks
{
    /// <summary>
    /// 通用任务处理策略
    /// </summary>
    public class TaskStrategy : ITaskStrategy<ITask>
    {
        private float m_Pro;

        public float Handle(ITask from, ITask task)
        {
            if (!task.IsStart)
            {
                task.OnUpdate();
            }

            if (task.IsComplete)
            {
                m_Pro = TaskBase.MAX_PRO;
            }
            else
            {
                m_Pro += 0.1f;
                m_Pro = Math.Min(m_Pro, 0.9f);
            }

            return m_Pro;
        }

        public void OnUse()
        {
            m_Pro = 0;
        }
    }
}

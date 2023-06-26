using System;

namespace XFrame.Modules.Tasks
{
    /// <summary>
    /// 通用任务处理策略
    /// </summary>
    public class TaskStrategy : ITaskStrategy<ITask>
    {
        private ITask m_Task;
        private float m_Pro;

        public float OnHandle(ITask from)
        {
            if (!m_Task.IsStart)
            {
                TaskModule.Inst.InnerExecTask(m_Task);
            }

            if (m_Task.IsComplete)
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

        public void OnUse(ITask task)
        {
            m_Pro = 0;
            m_Task = task;
        }

        public void OnFinish()
        {
            m_Task = null;
        }
    }
}

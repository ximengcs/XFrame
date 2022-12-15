using System;

namespace XFrame.Modules
{
    /// <summary>
    /// 通用任务处理策略
    /// </summary>
    public class TaskStrategy : ITaskStrategy
    {
        private Type m_Type = typeof(ITask);
        public Type HandleType => m_Type;

        public bool Handle(ITask from, ITaskHandler target)
        {
            ITask task = (ITask)target;
            if (task.IsStart)
            {
                task.OnUpdate();
                if (task.IsComplete)
                    return true;
            }
            return false;
        }
    }
}

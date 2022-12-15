using System;

namespace XFrame.Modules
{
    /// <summary>
    /// 空任务
    /// 这个任务总是处于完成状态
    /// </summary>
    public class EmptyTask : ITask
    {
        private Type m_HandleType = typeof(EmptyTask);
        private Action m_Complete;
        private Action<float> m_Update;
        public bool IsComplete => true;

        public bool IsStart { get; private set; }

        public Type HandleType => m_HandleType;

        public float Pro => 1;

        public ITask Add(ITaskHandler target)
        {
            return this;
        }

        public ITask AddStrategy(ITaskStrategy strategy)
        {
            return this;
        }

        public ITask OnComplete(Action complete)
        {
            m_Complete += complete;
            return this;
        }

        public ITask OnUpdate(Action<float> update)
        {
            m_Update += update;
            return this;
        }

        public void OnInit()
        {
        }

        public void OnUpdate()
        {
            
        }

        public void Start()
        {
            IsStart = true;
            m_Update?.Invoke(Pro);
            m_Complete?.Invoke();
            m_Complete = null;
        }
    }
}
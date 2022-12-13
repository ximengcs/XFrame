using System;

namespace XFrame.Modules
{
    public class EmptyTask : ITask
    {
        private Type m_HandleType = typeof(EmptyTask);
        private Action m_Complete;
        public bool IsComplete => true;

        public bool IsStart { get; private set; }

        public Type HandleType => m_HandleType;

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

        public void OnInit()
        {
        }

        public void OnUpdate()
        {
        }

        public void Start()
        {
            IsStart = true;
            m_Complete?.Invoke();
            m_Complete = null;
        }
    }
}
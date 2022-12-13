using System;
using System.Collections.Generic;

namespace XFrame.Modules
{
    public abstract class TaskBase : ITask
    {
        private ITaskHandler m_Current;
        private Action m_OnComplete;
        private Queue<ITaskHandler> m_Targets;
        private Dictionary<Type, ITaskStrategy> m_Strategys;

        public bool IsComplete { get; protected set; }

        public bool IsStart { get; protected set; }

        public abstract Type HandleType { get; }

        public ITask AddStrategy(ITaskStrategy strategy)
        {
            m_Strategys.Add(strategy.HandleType, strategy);
            return this;
        }

        public ITask Add(ITaskHandler data)
        {
            m_Targets.Enqueue(data);
            return this;
        }

        public ITask OnComplete(Action complete)
        {
            m_OnComplete += complete;
            return this;
        }

        public void OnUpdate()
        {
            if (m_Current != null)
            {
                if (m_Strategys.TryGetValue(m_Current.HandleType, out ITaskStrategy stategy))
                {
                    if (stategy.Handle(this, m_Current))
                    {
                        m_Current = null;
                        InnerCheckComplete();
                    }
                }
                else
                {
                    Log.Error("XFrame", "XTask Error");
                }
            }

            if (m_Targets.Count > 0)
            {
                if (m_Current == null)
                    m_Current = m_Targets.Dequeue();
            }
        }

        public virtual void OnInit()
        {
            m_OnComplete = null;
            m_Targets = new Queue<ITaskHandler>();
            m_Strategys = new Dictionary<Type, ITaskStrategy>();
        }

        public void Start()
        {
            IsStart = true;
        }

        private void InnerCheckComplete()
        {
            if (m_Targets.Count == 0)
            {
                InnerComplete();
            }
        }

        protected virtual void InnerComplete()
        {
            IsComplete = true;
            m_OnComplete?.Invoke();
            m_OnComplete = null;
        }
    }
}

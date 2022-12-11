using System;
using System.Collections.Generic;

namespace XFrame.Modules
{
    public partial class XTask
    {
        private Queue<object> m_Actions;
        private Action m_Callback;
        private object m_Current;

        public bool IsComplete { get; private set; }
        public bool IsStart { get; private set; }

        public XTask()
        {
            IsComplete = false;
            m_Actions = new Queue<object>();
        }

        public void Start()
        {
            IsStart = true;
        }

        public XTask Add(XTask task)
        {
            m_Actions.Enqueue(task);
            return this;
        }

        public XTask Add(Func<bool> taskHandler)
        {
            m_Actions.Enqueue(taskHandler);
            return this;
        }

        public XTask OnComplete(Action complete)
        {
            m_Callback = complete;
            return this;
        }

        public void Update()
        {
            if (m_Current != null)
            {
                if (m_Current is XTask task)
                {
                    if (task.IsStart)
                    {
                        task.Update();
                        if (task.IsComplete)
                        {
                            m_Current = null;
                            InnerCheckComplete();
                        }
                    }
                }
                else if (m_Current is Func<bool> handler)
                {
                    if (handler())
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

            if (m_Actions.Count > 0)
            {
                if (m_Current == null)
                    m_Current = m_Actions.Dequeue();
            }
        }

        private void InnerCheckComplete()
        {
            if (m_Actions.Count == 0)
            {
                IsComplete = true;
                m_Callback?.Invoke();
                m_Callback = null;
            }
        }
    }
}

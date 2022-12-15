using System;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;

namespace XFrame.Modules.Tasks
{
    /// <summary>
    /// 任务基类
    /// </summary>
    public abstract class TaskBase : ITask
    {
        internal const float MAX_PRO = 1;
        private ITaskHandler m_Current;
        private Action m_OnComplete;
        private Action<float> m_OnUpdate;
        private Queue<ITaskHandler> m_Targets;
        private Dictionary<Type, ITaskStrategy> m_Strategys;
        private float m_PerProRate;

        public bool IsComplete { get; protected set; }
        public bool IsStart { get; protected set; }
        public float Pro { get; protected set; }
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

        public ITask OnUpdate(Action<float> update)
        {
            m_OnUpdate += update;
            return this;
        }

        public void OnUpdate()
        {
            if (m_Current != null)
            {
                if (m_Strategys.TryGetValue(m_Current.HandleType, out ITaskStrategy stategy))
                {
                    float curPro = stategy.Handle(this, m_Current);
                    bool finish = curPro >= MAX_PRO;
                    curPro = Math.Min(curPro, MAX_PRO);
                    curPro = Math.Max(curPro, 0);
                    curPro *= m_PerProRate;
                    Pro += curPro;
                    Pro = Math.Min(Pro, MAX_PRO);
                    m_OnUpdate?.Invoke(Pro);

                    if (finish)
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
                {
                    m_Current = m_Targets.Dequeue();
                    InnerMarkUse();
                }
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
            m_PerProRate = MAX_PRO / m_Targets.Count;
        }

        private void InnerCheckComplete()
        {
            if (m_Targets.Count == 0)
            {
                Pro = MAX_PRO;
                InnerComplete();
            }
        }

        private void InnerMarkUse()
        {
            foreach (ITaskStrategy strategy in m_Strategys.Values)
                strategy.Use();
        }

        protected virtual void InnerComplete()
        {
            IsComplete = true;
            m_OnComplete?.Invoke();
            m_OnComplete = null;
        }
    }
}

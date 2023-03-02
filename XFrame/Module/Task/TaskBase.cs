using System;
using XFrame.Collections;
using System.Collections.Generic;

namespace XFrame.Modules.Tasks
{
    /// <summary>
    /// 任务基类
    /// </summary>
    public abstract partial class TaskBase : ITask
    {
        public const float MAX_PRO = 1;
        private ExecInfo m_Current;
        private Action m_OnComplete;
        private Action<float> m_OnUpdate;
        private Queue<ITaskHandler> m_Targets;
        private float m_PerProRate;
        private Type HandlerTypeBase;

        private XNode<StrategyInfo> m_Infos;

        public bool IsComplete { get; protected set; }
        public bool IsStart { get; protected set; }
        public float Pro { get; protected set; }

        public ITask AddStrategy(ITaskStrategy strategy)
        {
            Type type = strategy.GetType();
            StrategyInfo info = new StrategyInfo();
            info.Inst = strategy;

            Type interfaceType = type.GetInterface(HandlerTypeBase.FullName);
            info.HandleType = interfaceType.GetGenericArguments()[0];
            info.HandleMethod = type.GetMethod("Handle");
            m_Infos.Add((node) => node.Value.IsSub(info), info);
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

        void ITask.OnUpdate()
        {
            if (m_Current != null)
            {
                float curPro = m_Current.Exec(this);
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
                if (m_Targets.Count > 0)
                {
                    Console.WriteLine("count " + m_Targets.Count);
                    m_Current = new ExecInfo();
                    m_Current.Init(m_Targets.Dequeue(), m_Infos);
                }
            }
        }

        void ITask.OnInit()
        {
            m_OnComplete = null;
            HandlerTypeBase = typeof(ITaskStrategy<>);
            m_Targets = new Queue<ITaskHandler>();
            m_Infos = new XNode<StrategyInfo>();
            OnInit();
        }

        protected abstract void OnInit();

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

        protected virtual void InnerComplete()
        {
            IsComplete = true;
            m_OnComplete?.Invoke();
            m_OnComplete = null;
        }
    }
}

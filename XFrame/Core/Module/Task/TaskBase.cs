using System;
using XFrame.Collections;
using System.Threading.Tasks;
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

        private float m_Pro;
        private float m_CurPro;

        private XNode<StrategyInfo> m_Infos;

        public bool IsComplete { get; protected set; }
        public bool IsStart { get; protected set; }
        public float Pro => m_Pro + m_CurPro;

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
                m_CurPro = m_Current.Exec(this);
                bool finish = m_CurPro >= MAX_PRO;
                m_CurPro = Math.Min(m_CurPro, MAX_PRO);
                m_CurPro = Math.Max(m_CurPro, 0);
                m_CurPro *= m_PerProRate;
                m_OnUpdate?.Invoke(Pro);

                if (finish)
                {
                    m_Current = null;
                    m_Pro += m_CurPro;
                    m_CurPro = 0;
                    InnerCheckComplete();
                }
            }
            else
            {
                if (m_Targets.Count > 0)
                {
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
            AddStrategy(new TaskStrategy());
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
                m_Pro = MAX_PRO;
                m_CurPro = 0;
                InnerComplete();
            }
        }

        protected virtual void InnerComplete()
        {
            IsComplete = true;
            m_OnComplete?.Invoke();
            m_OnComplete = null;
        }

        public virtual Task Coroutine()
        {
            Task task = new Task(() => { });
            if (IsComplete)
                task.Start();
            else
                m_OnComplete += () => task.Start();
            Start();
            return task;
        }
    }
}

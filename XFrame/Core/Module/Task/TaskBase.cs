using System;
using XFrame.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Tasks
{
    /// <summary>
    /// 任务基类
    /// </summary>
    public abstract partial class TaskBase : ITask
    {
        public const float MAX_PRO = 1;
        private StrategyInfo m_Current;
        private Action m_OnComplete;
        private Action<float> m_OnUpdate;
        private Queue<ITaskHandler> m_Targets;
        private float m_PerProRate;
        private Type HandlerTypeBase;
        private XLinkList<Task> m_CorTasks;

        private float m_Pro;
        private float m_CurPro;

        private XNode<StrategyInfo> m_Infos;

        public string Name { get; private set; }
        public bool IsComplete { get; protected set; }
        public bool IsStart { get; protected set; }
        public float Pro => m_Pro + m_CurPro;

        public ITask AddStrategy(ITaskStrategy strategy)
        {
            StrategyInfo info = new StrategyInfo(strategy, HandlerTypeBase);
            m_Infos.Add((node) => node.Value.IsSub(info), info);
            return this;
        }

        public ITask Add(ITaskHandler data)
        {
            m_Targets.Enqueue(data);
            m_PerProRate = MAX_PRO / m_Targets.Count;
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
                m_CurPro = m_Current.Handle(this);
                bool finish = m_CurPro >= MAX_PRO;
                m_CurPro = Math.Min(m_CurPro, MAX_PRO);
                m_CurPro = Math.Max(m_CurPro, 0);
                m_CurPro *= m_PerProRate;
                m_OnUpdate?.Invoke(Pro);

                if (finish)
                {
                    m_Current.Finish();
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
                    ITaskHandler handler = m_Targets.Dequeue();
                    m_Current = InnerFindStrategy(handler);
                    m_Current.Use(handler);
                }
                else
                {
                    InnerCheckComplete();
                }
            }
        }

        private StrategyInfo InnerFindStrategy(ITaskHandler handler)
        {
            StrategyInfo result = null;
            Type handType = handler.GetType();

            StrategyInfo max = null;
            int level = 0;

            m_Infos.ForEachAll((n) =>
            {
                StrategyInfo info = n.Value;
                if (handType == info.HandleType)
                {
                    result = info;
                    max = null;
                    return false;
                }
                else if (info.IsSub(handType))
                {
                    if (max == null || n.Level > level)
                    {
                        max = info;
                        level = n.Level;
                    }
                }
                return true;
            });

            if (max != null)
                result = max;

            return result;
        }

        void ITask.OnInit(string name)
        {
            Name = name;
            m_OnComplete = null;
            OnInit();
        }

        void IPoolObject.OnCreate()
        {
            m_CorTasks = new XLinkList<Task>();
            HandlerTypeBase = typeof(ITaskStrategy<>);
            m_Targets = new Queue<ITaskHandler>();
            m_Infos = new XNode<StrategyInfo>();
            AddStrategy(new TaskStrategy());
            OnCreateFromPool();
        }

        void IPoolObject.OnRequest()
        {
            OnRequestFromPool();
        }

        void IPoolObject.OnRelease()
        {
            OnReleaseFromPool();
            InnerClearState();
        }

        void IPoolObject.OnDelete()
        {
            OnDestroyFromPool();
            InnerClearState();
        }

        private void InnerClearState()
        {
            m_Targets.Clear();
            m_CorTasks.Clear();
            m_OnComplete = null;
            m_OnUpdate = null;
            m_Current = null;
            m_PerProRate = 0;
            m_Pro = 0;
            m_CurPro = 0;
        }

        protected abstract void OnInit();
        protected virtual void OnCreateFromPool() { }
        protected virtual void OnRequestFromPool() { }
        protected virtual void OnDestroyFromPool() { }
        protected virtual void OnReleaseFromPool() { }

        public void Start()
        {
            if (IsStart)
                return;
            IsStart = true;
        }

        public void Delete()
        {
            foreach (XLinkNode<Task> node in m_CorTasks)
                node.Value.Dispose();
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
            XLinkNode<Task> node = m_CorTasks.AddLast(task);
            if (IsComplete)
                task.Start();
            else
            {
                m_OnComplete += () =>
                {
                    node.Delete();
                    task.Start();
                };
            }

            Start();
            return task;
        }
    }
}

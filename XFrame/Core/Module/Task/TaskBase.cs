using System;
using XFrame.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using XFrame.Modules.Pools;
using XFrame.Core;

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
        private Action m_OnCompleteAfter;
        private Action<ITask> m_OnComplete2;
        private Action<ITask> m_OnCompleteAfter2;
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

        IPool IPoolObject.InPool { get; set; }
        public string MarkName { get; set; }

        public ITask AddStrategy(ITaskStrategy strategy)
        {
            StrategyInfo info = new StrategyInfo(strategy, HandlerTypeBase);
            m_Infos.Add((node) => node.Value.IsSub(info), info);
            return this;
        }

        public ITaskStrategy GetStrategy(Type handleType)
        {
            XNode<StrategyInfo> resultNode = m_Infos.Get((node) =>
            {
                if (node.Value.HandleType == handleType)
                    return true;
                return false;
            });

            if (resultNode != null)
                return resultNode.Value.Inst;
            return null;
        }

        public virtual ITask Add(ITaskHandler data)
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

        public ITask OnCompleteAfter(Action complete)
        {
            m_OnCompleteAfter += complete;
            return this;
        }

        public ITask OnComplete(Action<ITask> complete)
        {
            m_OnComplete2 += complete;
            return this;
        }

        public ITask OnCompleteAfter(Action<ITask> complete)
        {
            m_OnCompleteAfter2 += complete;
            return this;
        }

        public ITask OnUpdate(Action<float> update)
        {
            m_OnUpdate += update;
            return this;
        }

        void ITask.OnUpdate()
        {
            InnerUpdate();
        }

        private void InnerUpdate()
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

                    if (m_Targets.Count > 0)
                    {
                        TaskModule taskModule = (TaskModule)XModule.Task;
                        if (taskModule.InnerCanContinue())
                            InnerUpdate();
                    }
                    else
                    {
                        InnerCheckComplete();
                    }
                }
            }
            else
            {
                if (m_Targets.Count > 0)
                {
                    ITaskHandler handler = m_Targets.Dequeue();
                    m_Current = InnerFindStrategy(handler);
                    m_Current.Use(handler);
                    InnerUpdate();
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
            m_OnComplete2 = null;
            m_OnCompleteAfter = null;
            m_OnCompleteAfter2 = null;
            OnInit();
        }

        int IPoolObject.PoolKey => 0;

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
            foreach (XLinkNode<Task> node in m_CorTasks)
                node.Value.Dispose();
            m_Targets.Clear();
            m_CorTasks.Clear();
            IsStart = false;
            IsComplete = false;
            m_OnComplete = null;
            m_OnCompleteAfter = null;
            m_OnComplete2 = null;
            m_OnCompleteAfter2 = null;
            m_OnUpdate = null;
            m_Current = null;
            m_PerProRate = 0;
            m_Pro = 0;
            m_CurPro = 0;
        }

        protected virtual void OnInit() { }
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

        private void InnerCheckComplete()
        {
            if (IsComplete)
                return;
            if (m_Targets.Count == 0)
            {
                m_Pro = MAX_PRO;
                m_CurPro = 0;
                m_OnUpdate?.Invoke(Pro);
                InnerComplete();
                InnerCompleteAfter();
            }
        }

        protected virtual void InnerComplete()
        {
            IsComplete = true;
            m_OnComplete?.Invoke();
            m_OnComplete2?.Invoke(this);
            m_OnComplete = null;
            m_OnComplete2 = null;
        }

        protected virtual void InnerCompleteAfter()
        {
            m_OnCompleteAfter?.Invoke();
            m_OnCompleteAfter2?.Invoke(this);
            m_OnCompleteAfter = null;
            m_OnCompleteAfter2 = null;
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

﻿using System;
using System.Threading.Tasks;
using XFrame.Core;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Tasks
{
    /// <summary>
    /// 空任务
    /// 这个任务总是处于完成状态
    /// </summary>
    public class EmptyTask : Singleton<EmptyTask>, ITask
    {
        private Action m_Complete;
        private Action<float> m_Update;
        public bool IsComplete => true;

        public bool IsStart { get; private set; }

        public float Pro => 1;

        public string Name { get; private set; }

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

        void ITask.OnInit(string name)
        {
            Name = name;
        }

        void ITask.OnUpdate()
        {

        }

        public void Start()
        {
            IsStart = true;
            m_Update?.Invoke(Pro);
            m_Complete?.Invoke();
            m_Complete = null;
        }

        public Task Coroutine()
        {
            return Task.CompletedTask;
        }

        public void Delete()
        {

        }

        int IPoolObject.PoolKey => 0;

        void IPoolObject.OnCreate()
        {

        }

        void IPoolObject.OnRequest()
        {

        }

        void IPoolObject.OnRelease()
        {
            m_Complete = null;
            m_Update = null;
        }

        void IPoolObject.OnDelete()
        {

        }
    }
}
using System;
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
        private Action m_CompleteAfter;
        private Action<ITask> m_Complete2;
        private Action<ITask> m_CompleteAfter2;
        private Action<float> m_Update;

        IPool IPoolObject.InPool { get; set; }

        public string MarkName { get; set; }

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

        public ITaskStrategy GetStrategy(Type handleType)
        {
            return null;
        }

        public ITask OnComplete(Action complete)
        {
            m_Complete += complete;
            return this;
        }

        public ITask OnCompleteAfter(Action complete)
        {
            m_CompleteAfter += complete;
            return this;
        }

        public ITask OnComplete(Action<ITask> complete)
        {
            m_Complete2 += complete;
            return this;
        }

        public ITask OnCompleteAfter(Action<ITask> complete)
        {
            m_CompleteAfter2 += complete;
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
            m_Complete2?.Invoke(this);
            m_CompleteAfter?.Invoke();
            m_CompleteAfter2?.Invoke(this);
            m_Complete = null;
            m_Complete2 = null;
            m_CompleteAfter = null;
            m_CompleteAfter2 = null;
        }

        public Task Coroutine()
        {
            return Task.CompletedTask;
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
            m_Complete2 = null;
            m_CompleteAfter = null;
            m_CompleteAfter2 = null;
            m_Update = null;
        }

        void IPoolObject.OnDelete()
        {

        }
    }
}
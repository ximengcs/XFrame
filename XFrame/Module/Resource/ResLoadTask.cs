
using System;

namespace XFrame.Modules
{
    public class ResLoadTask : TaskBase
    {
        private class Strategy : ITaskStrategy
        {
            private Type m_Type = typeof(ResLoadTask);
            public Type HandleType => m_Type;

            public bool Handle(ITask from, ITaskHandler target)
            {
                IResHandler hander = target as IResHandler;
                if (hander.IsDone)
                {
                    ResLoadTask task = from as ResLoadTask;
                    task.Res = hander.Data;
                    return true;
                }
                return false;
            }
        }

        private Type m_HandleType = typeof(ResLoadTask);
        private Action<object> m_Callback;

        public object Res { get; private set; }

        public override Type HandleType => m_HandleType;

        public override void OnInit()
        {
            base.OnInit();
            AddStrategy(new Strategy());
        }

        public ITask OnComplete(Action<object> callback)
        {
            m_Callback += callback;
            return this;
        }

        protected override void InnerComplete()
        {
            base.InnerComplete();
            m_Callback?.Invoke(Res);
            m_Callback = null;
        }
    }

    public class ResLoadTask<T> : TaskBase
    {
        private class Strategy : ITaskStrategy
        {
            private Type m_Type = typeof(ResLoadTask);
            public Type HandleType => m_Type;

            public bool Handle(ITask from, ITaskHandler target)
            {
                IResHandler hander = target as IResHandler;
                if (hander.IsDone)
                {
                    ResLoadTask<T> task = from as ResLoadTask<T>;
                    task.Res = (T)hander.Data;
                    return true;
                }
                return false;
            }
        }

        private Type m_HandleType = typeof(ResLoadTask);
        private Action<T> m_Callback;

        public override Type HandleType => m_HandleType;

        public override void OnInit()
        {
            base.OnInit();
            AddStrategy(new Strategy());
        }

        public T Res { get; private set; }

        public ITask OnComplete(Action<T> callback)
        {
            m_Callback += callback;
            return this;
        }

        protected override void InnerComplete()
        {
            base.InnerComplete();
            m_Callback?.Invoke(Res);
            m_Callback = null;
        }
    }
}

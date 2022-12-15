
using System;

namespace XFrame.Modules
{
    /// <summary>
    /// 资源加载任务
    /// </summary>
    public class ResLoadTask : TaskBase
    {
        private class Strategy : ITaskStrategy
        {
            private float m_Pro;
            private Type m_Type = typeof(ResLoadTask);
            public Type HandleType => m_Type;

            public float Handle(ITask from, ITaskHandler target)
            {
                IResHandler hander = target as IResHandler;
                if (hander.IsDone)
                {
                    ResLoadTask task = from as ResLoadTask;
                    task.Res = hander.Data;
                    m_Pro = MAX_PRO;
                }
                else
                {
                    m_Pro = hander.Pro;
                }
                return m_Pro;
            }

            public void Use()
            {
                m_Pro = 0;
            }
        }

        private Type m_HandleType = typeof(ResLoadTask);
        private Action<object> m_Callback;

        /// <summary>
        /// 加载到的资源
        /// </summary>
        public object Res { get; private set; }

        /// <summary>
        /// 资源加载处理器类 IResHandler类
        /// </summary> 
        public override Type HandleType => m_HandleType;

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        public override void OnInit()
        {
            base.OnInit();
            AddStrategy(new Strategy());
        }

        /// <summary>
        /// 设置完成回调
        /// </summary>
        /// <param name="callback">回调</param>
        /// <returns>资源加载任务</returns>
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

    /// <summary>
    /// 资源加载任务
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    public class ResLoadTask<T> : TaskBase
    {
        private class Strategy : ITaskStrategy
        {
            private float m_Pro;
            private Type m_Type = typeof(ResLoadTask);
            public Type HandleType => m_Type;

            public float Handle(ITask from, ITaskHandler target)
            {
                IResHandler hander = target as IResHandler;
                if (hander.IsDone)
                {
                    ResLoadTask<T> task = from as ResLoadTask<T>;
                    task.Res = (T)hander.Data;
                    m_Pro = MAX_PRO;
                }
                else
                {
                    m_Pro = hander.Pro;
                }

                return m_Pro;
            }

            public void Use()
            {
                m_Pro = 0;
            }
        }

        private Type m_HandleType = typeof(ResLoadTask);
        private Action<T> m_Callback;

        /// <summary>
        /// 资源加载处理器类 IResHandler类
        /// </summary> 
        public override Type HandleType => m_HandleType;

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        public override void OnInit()
        {
            base.OnInit();
            AddStrategy(new Strategy());
        }

        /// <summary>
        /// 加载到的资源
        /// </summary>
        public T Res { get; private set; }

        /// <summary>
        /// 设置加载回调
        /// </summary>
        /// <param name="callback">回调</param>
        /// <returns>资源加载任务</returns>
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

using System;
using XFrame.Modules.Tasks;
#if OLD_TASK
namespace XFrame.Modules.Resource
{
    /// <summary>
    /// 资源加载任务
    /// </summary>
    public class ResLoadTask : TaskBase
    {
        private class Strategy : ITaskStrategy<IResHandler>
        {
            private IResHandler m_Handler;
            private float m_Pro;

            public float OnHandle(ITask from)
            {
                if (m_Handler.IsDone)
                    m_Pro = MAX_PRO;
                else
                    m_Pro = m_Handler.Pro;

                if (m_Handler.IsDone || m_Handler.Pro == MAX_PRO)
                {
                    ResLoadTask task = from as ResLoadTask;
                    task.Res = m_Handler.Data;
                }

                return m_Pro;
            }

            public void OnUse(IResHandler handler)
            {
                m_Pro = 0;
                m_Handler = handler;
                m_Handler.Start();
            }

            public void OnFinish()
            {
                m_Handler.Dispose();
                m_Handler = null;
            }
        }

        private Action<object> m_Callback;

        /// <summary>
        /// 加载到的资源
        /// </summary>
        public object Res { get; private set; }

        protected override void OnCreateFromPool()
        {
            base.OnCreateFromPool();
            AddStrategy(new Strategy());
        }

        protected override void OnReleaseFromPool()
        {
            base.OnReleaseFromPool();
            Res = null;
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
        private class Strategy : ITaskStrategy<IResHandler>
        {
            private IResHandler m_Handler;
            private float m_Pro;

            public float OnHandle(ITask from)
            {
                if (m_Handler.IsDone)
                    m_Pro = MAX_PRO;
                else
                    m_Pro = m_Handler.Pro;

                if (m_Handler.IsDone || m_Handler.Pro == MAX_PRO)
                {
                    ResLoadTask<T> task = from as ResLoadTask<T>;
                    task.Res = (T)m_Handler.Data;
                }

                return m_Pro;
            }

            public void OnUse(IResHandler hander)
            {
                m_Pro = 0;
                m_Handler = hander;
                m_Handler.Start();
            }

            public void OnFinish()
            {
                m_Handler.Dispose();
                m_Handler = null;
            }
        }

        private Action<T> m_Callback;

        protected override void OnCreateFromPool()
        {
            base.OnCreateFromPool();
            AddStrategy(new Strategy());
        }

        protected override void OnReleaseFromPool()
        {
            base.OnReleaseFromPool();
            Res = default;
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
#endif
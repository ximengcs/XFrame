using System;
using XFrame.Modules.Pools;

namespace XFrame.Tools
{
    /// <summary>
    /// 初始化检查器
    /// </summary>
    public class InitChecker : PoolObjectBase, IPoolObject
    {
        private bool m_Inited;
        private Action m_Callback;

        private InitChecker() { }

        /// <summary>
        /// 触发初始化成功
        /// </summary>
        public void Trigger()
        {
            if (m_Inited) return;
            m_Inited = true;
            m_Callback?.Invoke();
            m_Callback = null;
        }

        /// <summary>
        /// 监听触发
        /// </summary>
        /// <param name="callback">处理函数</param>
        public void Listen(Action callback)
        {
            if (m_Inited)
            {
                callback?.Invoke();
            }
            else
            {
                m_Callback += callback;
            }
        }

        /// <inheritdoc/>
        protected internal override void OnReleaseFromPool()
        {
            base.OnReleaseFromPool();
            m_Inited = false;
            m_Callback = null;
        }

        /// <inheritdoc/>
        protected internal override void OnRequestFromPool()
        {
            base.OnRequestFromPool();
            m_Inited = false;
            m_Callback = null;
        }

        /// <summary>
        /// 创建初始化检查器
        /// </summary>
        /// <returns></returns>
        public static InitChecker Create()
        {
            InitChecker inst = References.Require<InitChecker>();
            return inst;
        }

        /// <summary>
        /// 获取检查器状态
        /// </summary>
        /// <param name="checker">检查器</param>
        public static implicit operator bool(InitChecker checker)
        {
            return checker.m_Inited;
        }
    }
}

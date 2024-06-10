using System;

namespace XFrame.Tasks
{
    /// <summary>
    /// 取消绑定器
    /// </summary>
    public partial class XTaskCancelToken
    {
        private bool m_Canceled;
        private Action m_Handler;
        private bool m_Disposed;

        /// <summary>
        /// 是否已经销毁
        /// </summary>
        public bool Disposed => m_Disposed;

        /// <summary>
        /// 是否取消
        /// </summary>
        public bool Canceled => m_Canceled;

        private XTaskCancelToken()
        {
        }

        /// <summary>
        /// 取消
        /// </summary>
        public void Cancel()
        {
            m_Canceled = true;
        }

        /// <summary>
        /// 添加取消时的处理函数
        /// </summary>
        /// <param name="handler">处理函数</param>
        public void AddHandler(Action handler)
        {
            m_Handler += handler;
        }

        /// <summary>
        /// 移除处理函数
        /// </summary>
        /// <param name="handler"></param>
        public void RemoveHandler(Action handler)
        {
            m_Handler -= handler;
        }

        /// <summary>
        /// 释放到池中
        /// </summary>
        public void Dispose()
        {
            m_Disposed = true;
            Clear();
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            m_Handler = null;
        }

        /// <summary>
        /// 执行取消函数，且抛出异常
        /// </summary>
        public void Invoke()
        {
            if (m_Canceled)
            {
                m_Handler?.Invoke();
                InnerThrowOperationCanceledException();
            }
        }

        /// <summary>
        /// 执行取消函数，不抛出异常
        /// </summary>
        public void InvokeWithoutException()
        {
            if (m_Canceled)
            {
                m_Handler?.Invoke();
            }
        }

        private void InnerThrowOperationCanceledException()
        {
            throw new OperationCanceledException("OperationCanceled");
        }
    }
}
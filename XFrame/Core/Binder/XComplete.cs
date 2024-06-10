using System;

namespace XFrame.Core.Binder
{
    /// <summary>
    /// 带标记的完成委托
    /// </summary>
    /// <typeparam name="T">持有数据类型</typeparam>
    public struct XComplete<T>
    {
        private bool m_Complete;
        private T m_Value;
        private Action m_Handler;
        private Action<T> m_Handler2;

        /// <summary>
        /// 持有值
        /// </summary>
        public T Value
        {
            get => m_Value;
            set => m_Value = value;
        }

        /// <summary>
        /// 是否已经完成，当设置为true时会自动调用完成回调
        /// </summary>
        public bool IsComplete
        {
            get => m_Complete;
            set
            {
                m_Complete = value;
                if (value)
                    Invoke();
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="defaultValue">持有默认值</param>
        public XComplete(T defaultValue)
        {
            m_Value = defaultValue;
            m_Complete = false;
            m_Handler = null;
            m_Handler2 = null;
        }

        /// <summary>
        /// 执行完成委托
        /// </summary>
        public void Invoke()
        {
            if (!m_Complete)
                return;
            if (m_Handler != null)
            {
                m_Handler.Invoke();
                m_Handler = null;
            }

            if (m_Handler2 != null)
            {
                m_Handler2.Invoke(m_Value);
                m_Handler2 = null;
            }
        }

        /// <summary>
        /// 注册完成回调
        /// </summary>
        /// <param name="handler">回调</param>
        public void On(Action handler)
        {
            if (m_Complete)
                handler();
            else
                m_Handler += handler;
        }

        /// <summary>
        /// 注册带有值的完成回调
        /// </summary>
        /// <param name="handler">完成回调</param>
        public void On(Action<T> handler)
        {
            if (m_Complete)
                handler(m_Value);
            else
                m_Handler2 += handler;
        }
    }
}
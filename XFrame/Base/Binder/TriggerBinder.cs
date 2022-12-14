using System;
using System.Collections.Generic;

namespace XFrame.Core
{
    /// <summary>
    /// 数值绑定器
    /// </summary>
    /// <typeparam name="T">持有的数值类型</typeparam>
    public class TriggerBinder<T> : IDisposable
    {
        private Func<T> m_GetHandler;
        private Action<T> m_UpdateHandler;
        private List<Func<T, bool>> m_CondUpdateHandler;

        /// <summary>
        /// 构造数值绑定器
        /// </summary>
        /// <param name="getHandler">获取值的委托</param>
        public TriggerBinder(Func<T> getHandler)
        {
            m_GetHandler = getHandler;
            m_CondUpdateHandler = new List<Func<T, bool>>();
        }

        /// <summary>
        /// 持有的数值
        /// </summary>
        public T Value
        {
            get { return m_GetHandler(); }
        }

        /// <summary>
        /// 触发数值更新
        /// </summary>
        public void Trigger()
        {
            m_UpdateHandler?.Invoke(Value);

            for (int i = m_CondUpdateHandler.Count - 1; i >= 0; i--)
            {
                Func<T, bool> fun = m_CondUpdateHandler[i];
                if (fun == null || fun(Value))
                    m_CondUpdateHandler.RemoveAt(i);
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            m_GetHandler = null;
            m_UpdateHandler = null;
            m_CondUpdateHandler = null;
        }

        /// <summary>
        /// 添加一个数值变更处理委托
        /// </summary>
        /// <param name="handler">更新时的处理委托</param>
        /// <param name="atonceInvoke">是否立即执行</param>
        public void AddHandler(Action<T> handler, bool atonceInvoke = false)
        {
            m_UpdateHandler += handler;
            if (atonceInvoke)
                handler?.Invoke(Value);
        }

        /// <summary>
        /// 移除一个数值变更处理委托
        /// </summary>
        /// <param name="handler">要移除的委托</param>
        public void RemoveHandler(Action<T> handler)
        {
            m_UpdateHandler -= handler;
        }

        /// <summary>
        /// 添加一个带返回值的数值变更处理委托
        /// </summary>
        /// <param name="handler">需要添加的委托，当委托返回true时，在通知完后会移除掉该委托</param>
        public void AddCondHandler(Func<T, bool> handler)
        {
            m_CondUpdateHandler.Add(handler);
        }

        /// <summary>
        /// 移除一个带返回值的数值变更处理委托
        /// </summary>
        /// <param name="handler">需要移除的委托</param>
        public void RemoveCondHandler(Func<T, bool> handler)
        {
            m_CondUpdateHandler.Remove(handler);
        }

        public static implicit operator T(TriggerBinder<T> binder)
        {
            return binder.Value;
        }
    }
}
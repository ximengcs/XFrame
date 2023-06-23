﻿using System;
using System.Collections.Generic;
using XFrame.Collections;

namespace XFrame.Core.Binder
{
    /// <summary>
    /// 数值绑定器
    /// </summary>
    /// <typeparam name="T">持有数值类型</typeparam>
    public class ValueBinder<T> : IDisposable
    {
        #region Inner Fields
        private Func<T> m_GetHandler;
        private Action<T> m_SetHandler;
        private Action<T> m_UpdateHandler;
        private XLinkList<Func<T, bool>> m_CondUpdateHandler;
        #endregion

        #region Constructor
        /// <summary>
        /// 构造数值绑定器
        /// </summary>
        /// <param name="getHandler">获取值委托</param>
        /// <param name="setHandler">设置值委托</param>
        public ValueBinder(Func<T> getHandler, Action<T> setHandler)
        {
            m_GetHandler = getHandler;
            m_SetHandler = setHandler;
            m_CondUpdateHandler = new XLinkList<Func<T, bool>>();
        }
        #endregion

        #region Interface
        /// <summary>
        /// 持有的数值
        /// </summary>
        public T Value
        {
            get { return m_GetHandler(); }
            set
            {
                if (value != null)
                {
                    if (value.Equals(Value))
                        return;
                }
                else if (Value == null)
                {
                    return;
                }

                m_SetHandler.Invoke(value);
                m_UpdateHandler?.Invoke(Value);

                XLinkNode<Func<T, bool>> node = m_CondUpdateHandler.First;
                while (node != null)
                {
                    Func<T, bool> fun = node.Value;
                    if (fun(Value))
                    {
                        XLinkNode<Func<T, bool>> tmpNode = node.Next;
                        node.Delete();
                        node = tmpNode;
                    }
                    else
                    {
                        node = node.Next;
                    }
                }
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            m_GetHandler = null;
            m_SetHandler = null;
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
            if (handler == null)
                return;
            m_UpdateHandler += handler;
            if (atonceInvoke)
                handler.Invoke(Value);
        }

        /// <summary>
        /// 移除一个数值变更处理委托
        /// </summary>
        /// <param name="handler">要移除的委托</param>
        public void RemoveHandler(Action<T> handler)
        {
            if (handler == null)
                return;
            m_UpdateHandler -= handler;
        }

        /// <summary>
        /// 添加一个带返回值的数值变更处理委托
        /// </summary>
        /// <param name="handler">需要添加的委托，当委托返回true时，在通知完后会移除掉该委托</param>
        public void AddCondHandler(Func<T, bool> handler)
        {
            if (handler == null)
                return;
            if (!handler(Value))
                m_CondUpdateHandler.AddLast(handler);
        }

        /// <summary>
        /// 移除一个带返回值的数值变更处理委托
        /// </summary>
        /// <param name="handler">需要移除的委托</param>
        public void RemoveCondHandler(Func<T, bool> handler)
        {
            if (handler == null)
                return;
            m_CondUpdateHandler.Remove(handler);
        }

        public static implicit operator T(ValueBinder<T> binder)
        {
            return binder.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
        #endregion
    }
}
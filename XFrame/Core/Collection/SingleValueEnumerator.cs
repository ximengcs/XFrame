using System.Collections;
using System.Collections.Generic;

namespace XFrame.Collections
{
    /// <summary>
    /// 单值迭代器
    /// </summary>
    /// <typeparam name="T">持有值</typeparam>
    public struct SingleValueEnumerator<T> : IEnumerator<T>
    {
        private int m_Index;

        /// <summary>
        /// 当前值
        /// </summary>
        public T Current { get; }

        object IEnumerator.Current => Current;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="value">持有值</param>
        public SingleValueEnumerator(T value)
        {
            Current = value;
            m_Index = 0;
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {

        }

        /// <summary>
        /// 下一个
        /// </summary>
        /// <returns>是否还有下一个</returns>
        public bool MoveNext()
        {
            return m_Index++ == 0;
        }

        /// <summary>
        /// 重置迭代器
        /// </summary>
        public void Reset()
        {
            m_Index = 0;
        }
    }
}

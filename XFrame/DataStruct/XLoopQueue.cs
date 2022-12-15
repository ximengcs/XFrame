﻿using XFrame.Modules.Diagnotics;

namespace XFrame.Collections
{
    /// <summary>
    /// 循环队列
    /// </summary>
    /// <typeparam name="T">持有的数据类型</typeparam>
    public class XLoopQueue<T>
    {
        private int m_Capacity;
        private T[] m_Objects;
        private int m_L;
        private int m_R;

        /// <summary>
        /// 容量
        /// </summary>
        public int Capacity => m_Capacity;

        /// <summary>
        /// 队列是否空
        /// </summary>
        public bool Empty => m_L == m_R;

        /// <summary>
        /// 队列是否满
        /// </summary>
        public bool Full => (m_R + 1) % m_Capacity == m_L;

        /// <summary>
        /// 循环队列
        /// </summary>
        /// <param name="capacity">队列容量</param>
        public XLoopQueue(int capacity)
        {
            m_L = 0;
            m_R = 0;
            m_Capacity = capacity;
            m_Objects = new T[m_Capacity];
        }

        /// <summary>
        /// 从队列前端添加一个元素
        /// 如果队列满，则报告错误
        /// </summary>
        /// <param name="element">要添加的元素</param>
        public void AddFirst(T element)
        {
            if (Full)
            {
                Log.Error("XFrame", "XLoopQueue add first element error. queue is full.");
                return;
            }

            m_L = (m_L - 1 + m_Capacity) % m_Capacity;
            m_Objects[m_L] = element;
        }

        /// <summary>
        /// 从队列前端移除一个元素
        /// 如果队列空，则报告错误
        /// </summary>
        /// <returns>移除掉的元素</returns>
        public T RemoveFirst()
        {
            if (Empty)
            {
                Log.Error("XFrame", "XLoopQueue remove first element error. queue is empty.");
                return default;
            }

            T element = m_Objects[m_L];
            m_Objects[m_L] = default;
            m_L = (m_L + 1) % m_Capacity;
            return element;
        }

        /// <summary>
        /// 获取队列前端的第一个元素
        /// </summary>
        /// <returns>获取到的元素</returns>
        public T GetFirst()
        {
            return m_Objects[m_L];
        }

        /// <summary>
        /// 从队列后端添加一个元素
        /// 如果队列满，则报告错误
        /// </summary>
        /// <param name="element">要添加的元素</param>
        public void AddLast(T element)
        {
            if (Full)
            {
                Log.Error("XFrame", "XLoopQueue add last element error. queue is full.");
                return;
            }

            m_Objects[m_R] = element;
            m_R = (m_R + 1) % m_Capacity;
        }

        /// <summary>
        /// 从队列后端移除一个元素
        /// 如果队列空，则报告错误
        /// </summary>
        /// <returns>移除掉的元素</returns>
        public T RemoveLast()
        {
            if (Empty)
            {
                Log.Error("XFrame", "XLoopQueue remove last element error. queue is empty.");
                return default;
            }

            m_R = (m_R - 1 + m_Capacity) % m_Capacity;
            T element = m_Objects[m_L];
            m_Objects[m_R] = default;
            return element;
        }

        /// <summary>
        /// 获取队列后端的第一个元素
        /// </summary>
        /// <returns>获取到的元素</returns>
        public T GetLast()
        {
            return m_Objects[m_R];
        }
    }
}

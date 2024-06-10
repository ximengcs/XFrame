using System.Collections;
using System.Collections.Generic;

namespace XFrame.Collections
{
    /// <summary>
    /// 列表扩展类
    /// </summary>
    public class ListExt
    {
        /// <summary>
        /// 前向迭代器
        /// </summary>
        /// <typeparam name="T">持有数据类型</typeparam>
        public struct ForwardIt<T> : IEnumerator<T>
        {
            private List<T> m_List;
            private int m_Index;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="list">目标List</param>
            public ForwardIt(List<T> list)
            {
                m_List = list;
                m_Index = -1;
            }

            /// <summary>
            /// 当前值
            /// </summary>
            public T Current => m_List[m_Index];

            object IEnumerator.Current => Current;

            /// <summary>
            /// 移动至下一个
            /// </summary>
            /// <returns>是否还有元素</returns>
            public bool MoveNext()
            {
                m_Index++;
                return m_Index < m_List.Count;
            }

            /// <summary>
            /// 重置迭代器
            /// </summary>
            public void Reset()
            {
                m_Index = -1;
            }

            /// <summary>
            /// 释放
            /// </summary>
            public void Dispose()
            {
                m_Index = 0;
                m_List = null;
            }
        }

        /// <summary>
        /// 后向迭代器
        /// </summary>
        /// <typeparam name="T">数据持有类型</typeparam>
        public struct BackwardIt<T> : IEnumerator<T>
        {
            private List<T> m_List;
            private int m_Index;

            /// <summary>
            /// 构造器
            /// </summary>
            /// <param name="list">目标List</param>
            public BackwardIt(List<T> list)
            {
                m_List = list;
                m_Index = m_List.Count;
            }

            /// <summary>
            /// 当前值
            /// </summary>
            public T Current => m_List[m_Index];

            object IEnumerator.Current => Current;

            /// <summary>
            /// 下一个
            /// </summary>
            /// <returns>是否还有元素</returns>
            public bool MoveNext()
            {
                m_Index--;
                return m_Index >= 0;
            }

            /// <summary>
            /// 重置迭代器
            /// </summary>
            public void Reset()
            {
                m_Index = m_List.Count;
            }

            /// <summary>
            /// 释放
            /// </summary>
            public void Dispose()
            {
                m_Index = 0;
                m_List = null;
            }
        }
    }
}

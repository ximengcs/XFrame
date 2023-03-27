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

            public ForwardIt(List<T> list)
            {
                m_List = list;
                m_Index = -1;
            }

            public T Current => m_List[m_Index];

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                m_Index++;
                return m_Index < m_List.Count;
            }

            public void Reset()
            {
                m_Index = -1;
            }

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

            public BackwardIt(List<T> list)
            {
                m_List = list;
                m_Index = m_List.Count;
            }

            public T Current => m_List[m_Index];

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                m_Index--;
                return m_Index >= 0;
            }

            public void Reset()
            {
                m_Index = m_List.Count;
            }

            public void Dispose()
            {
                m_Index = 0;
                m_List = null;
            }
        }
    }
}

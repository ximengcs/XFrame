using System.Collections;
using System.Collections.Generic;

namespace XFrame.Collections
{
    public partial class XCollection<T>
    {
        /// <summary>
        /// 后向迭代器
        /// </summary>
        public struct BackEnumerator : IEnumerator<T>
        {
            private List<T> m_List;
            private int m_Index;

            internal BackEnumerator(List<T> list)
            {
                m_List = list;
                m_Index = m_List.Count;
            }

            /// <summary>
            /// 当前迭代到的元素
            /// </summary>
            public T Current => m_List[m_Index];

            object IEnumerator.Current => m_List[m_Index];

            /// <summary>
            /// 释放
            /// </summary>
            public void Dispose()
            {
                m_List = null;
                m_Index = default;
            }

            /// <summary>
            /// 迭代下一个
            /// </summary>
            /// <returns>是否还有下一个元素</returns>
            public bool MoveNext()
            {
                m_Index--;
                return m_Index >= 0 && m_Index < m_List.Count;
            }

            /// <summary>
            /// 重置迭代器
            /// </summary>
            public void Reset()
            {
                m_Index = m_List.Count;
            }
        }
    }
}

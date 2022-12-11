using System.Collections;
using System.Collections.Generic;

namespace XFrame.Collections
{
    public partial class XCollection<T>
    {
        public struct BackEnumerator : IEnumerator<T>
        {
            private List<T> m_List;
            private int m_Index;

            internal BackEnumerator(List<T> list)
            {
                m_List = list;
                m_Index = m_List.Count;
            }

            public T Current => m_List[m_Index];

            object IEnumerator.Current => m_List[m_Index];

            public void Dispose()
            {
                m_List = null;
                m_Index = default;
            }

            public bool MoveNext()
            {
                m_Index--;
                return m_Index >= 0 && m_Index < m_List.Count;
            }

            public void Reset()
            {
                m_Index = m_List.Count;
            }
        }
    }
}

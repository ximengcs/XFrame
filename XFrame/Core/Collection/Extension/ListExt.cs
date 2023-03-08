
using System.Collections;
using System.Collections.Generic;

namespace XFrame.Collections
{
    public class ListExt
    {
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

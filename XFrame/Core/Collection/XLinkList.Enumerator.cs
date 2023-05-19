using System.Collections;
using System.Collections.Generic;

namespace XFrame.Collections
{
    public partial class XLinkList<T>
    {
        private struct ForwardIt : IEnumerator<XLinkNode<T>>
        {
            private XLinkNode<T>[] m_Cache;
            private int m_Index;

            public XLinkNode<T> Current => m_Cache[m_Index];

            object IEnumerator.Current => Current;

            public ForwardIt(XLinkList<T> list)
            {
                m_Cache = new XLinkNode<T>[list.Count];
                int index = 0;
                XLinkNode<T> node = list.First;
                while (node != null)
                {
                    m_Cache[index++] = node;
                    node = node.Next;
                }
                m_Index = -1;
            }

            public bool MoveNext()
            {
                while (m_Index >= 0 && m_Index < m_Cache.Length && m_Cache[m_Index].m_List == null)
                    m_Index++;
                return ++m_Index < m_Cache.Length;
            }

            public void Dispose()
            {
                m_Cache = null;
            }

            public void Reset()
            {
                m_Index = 0;
            }
        }

        private struct BackwardIt : IEnumerator<XLinkNode<T>>
        {
            private XLinkNode<T>[] m_Cache;
            private int m_Index;

            public XLinkNode<T> Current => m_Cache[m_Index];

            object IEnumerator.Current => Current;

            public BackwardIt(XLinkList<T> list)
            {
                m_Cache = new XLinkNode<T>[list.Count];
                int index = 0;
                XLinkNode<T> node = list.First;
                while (node != null)
                {
                    m_Cache[index++] = node;
                    node = node.Next;
                }
                m_Index = m_Cache.Length;
            }

            public bool MoveNext()
            {
                while (m_Index >= 0 && m_Index < m_Cache.Length && m_Cache[m_Index].m_List == null)
                    m_Index--;
                return --m_Index >= 0;
            }

            public void Dispose()
            {
                m_Cache = null;
            }

            public void Reset()
            {
                m_Index = 0;
            }
        }
    }
}

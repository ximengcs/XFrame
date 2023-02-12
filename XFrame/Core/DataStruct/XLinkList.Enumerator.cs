using System.Collections;
using System.Collections.Generic;

namespace XFrame.Collections
{
    public partial class XLinkList<T>
    {
        private struct ForwardIt : IEnumerator<T>
        {
            private XLinkList<T> m_List;
            private XLinkNode<T> m_Node;
            private bool m_Start;

            public T Current => m_Node.Value;

            object IEnumerator.Current => m_Node.Value;

            public ForwardIt(XLinkList<T> list)
            {
                m_Start = true;
                m_List = list;
                m_Node = null;
            }

            public bool MoveNext()
            {
                if (!m_Start)
                    return false;

                if (m_Node == null)
                    m_Node = m_List.First;
                else
                    m_Node = m_Node.Next;
                bool hasNext = m_Node != null;
                if (!hasNext)
                    m_Start = false;
                return hasNext;
            }

            public void Dispose()
            {
                m_List = null;
                m_Node = null;
            }

            public void Reset()
            {
                m_Start = true;
                m_Node = null;
            }
        }

        private struct BackwardIt : IEnumerator<T>
        {
            private XLinkList<T> m_List;
            private XLinkNode<T> m_Node;
            private bool m_Start;

            public T Current => m_Node.Value;

            object IEnumerator.Current => m_Node.Value;

            public BackwardIt(XLinkList<T> list)
            {
                m_Start = true;
                m_List = list;
                m_Node = null;
            }

            public bool MoveNext()
            {
                if (!m_Start)
                    return false;

                if (m_Node == null)
                    m_Node = m_List.Last;
                else
                    m_Node = m_Node.Pre;
                bool hasPre = m_Node != null;
                if (!hasPre)
                    m_Start = false;
                return hasPre;
            }

            public void Dispose()
            {
                m_List = null;
                m_Node = null;
            }

            public void Reset()
            {
                m_Start = true;
                m_Node = null;
            }
        }
    }
}

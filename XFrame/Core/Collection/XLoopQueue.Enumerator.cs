using System.Collections;
using System.Collections.Generic;

namespace XFrame.Collections
{
    public partial class XLoopQueue<T>
    {
        private struct ForwardIt : IEnumerator<T>
        {
            private XLoopQueue<T> m_Queue;
            private int m_Index;

            public T Current => m_Queue.m_Objects[m_Index];

            object IEnumerator.Current => Current;

            public ForwardIt(XLoopQueue<T> queue)
            {
                m_Queue = queue;
                m_Index = queue.m_L;
            }

            public bool MoveNext()
            {
                m_Index = (m_Index + 1) % m_Queue.m_Capacity;
                return m_Index < m_Queue.m_R;
            }

            public void Reset()
            {
                m_Index = m_Queue.m_L;
            }

            public void Dispose()
            {
                m_Queue = null;
                m_Index = 0;
            }
        }

        private struct BackwardIt : IEnumerator<T>
        {
            private XLoopQueue<T> m_Queue;
            private int m_Index;

            public T Current => m_Queue.m_Objects[m_Index];

            object IEnumerator.Current => Current;

            public BackwardIt(XLoopQueue<T> queue)
            {
                m_Queue = queue;
                m_Index = queue.m_R;
            }

            public bool MoveNext()
            {
                m_Index = (m_Index - 1 + m_Queue.m_Capacity) % m_Queue.m_Capacity;
                return m_Index > m_Queue.m_L;
            }

            public void Reset()
            {
                m_Index = m_Queue.m_R;
            }

            public void Dispose()
            {
                m_Queue = null;
                m_Index = 0;
            }
        }
    }
}

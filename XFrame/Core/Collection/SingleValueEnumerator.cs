using System.Collections;
using System.Collections.Generic;

namespace XFrame.Collections
{
    public struct SingleValueEnumerator<T> : IEnumerator<T>
    {
        private int m_Index;

        public T Current { get; }

        object IEnumerator.Current => Current;

        public SingleValueEnumerator(T value)
        {
            Current = value;
            m_Index = 0;
        }

        public void Dispose()
        {

        }

        public bool MoveNext()
        {
            return m_Index++ == 0;
        }

        public void Reset()
        {
            m_Index = 0;
        }
    }
}

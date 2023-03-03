using System.Collections;
using System.Collections.Generic;

namespace XFrame.Collections
{
    public partial class Csv
    {
        private struct ForwardIt : IEnumerator<Line>
        {
            private Line[] m_Lines;
            private int m_Index;

            public Line Current => m_Lines[m_Index];

            object IEnumerator.Current => Current;

            public ForwardIt(Line[] line)
            {
                m_Lines = line;
                m_Index = -1;
            }

            public bool MoveNext()
            {
                m_Index++;
                return m_Index < m_Lines.Length;
            }

            public void Reset()
            {
                m_Index = -1;
            }

            public void Dispose()
            {
                m_Lines = null;
                m_Index = 0;
            }
        }

        private struct BackwardIt : IEnumerator<Line>
        {
            private Line[] m_Lines;
            private int m_Index;

            public Line Current => m_Lines[m_Index];

            object IEnumerator.Current => Current;

            public BackwardIt(Line[] line)
            {
                m_Lines = line;
                m_Index = m_Lines.Length;
            }

            public bool MoveNext()
            {
                m_Index--;
                return m_Index >= 0;
            }

            public void Reset()
            {
                m_Index = m_Lines.Length;
            }

            public void Dispose()
            {
                m_Lines = null;
                m_Index = 0;
            }
        }
    }
}

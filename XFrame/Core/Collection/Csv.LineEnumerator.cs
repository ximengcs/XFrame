using System.Collections;
using System.Collections.Generic;

namespace XFrame.Collections
{
    public partial class Csv
    {
        private struct LineForwardIt : IEnumerator<string>
        {
            private string[] m_Lines;
            private int m_Index;

            public string Current => m_Lines[m_Index];

            object IEnumerator.Current => Current;

            public LineForwardIt(string[] line)
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

        private struct LineBackwardIt : IEnumerator<string>
        {
            private string[] m_Lines;
            private int m_Index;

            public string Current => m_Lines[m_Index];

            object IEnumerator.Current => Current;

            public LineBackwardIt(string[] line)
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

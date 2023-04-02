using System.Collections;
using System.Collections.Generic;

namespace XFrame.Collections
{
    public partial class Csv<T>
    {
        private struct Enumerator : IEnumerator<Line>
        {
            private IEnumerator<XLinkNode<Line>> m_Lines;

            public Line Current => m_Lines.Current.Value;

            object IEnumerator.Current => Current;

            public Enumerator(XLinkList<Line> line)
            {
                m_Lines = line.GetEnumerator();
            }

            public bool MoveNext()
            {
                return m_Lines.MoveNext();
            }

            public void Reset()
            {
                m_Lines.Reset();
            }

            public void Dispose()
            {
                m_Lines.Dispose();
            }
        }
    }
}

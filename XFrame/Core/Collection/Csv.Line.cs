
using System.Collections.Generic;
using System.Text;

namespace XFrame.Collections
{
    public partial class Csv<T>
    {
        public class Line : IXEnumerable<T>
        {
            private XItType m_ItType;
            private T[] m_Data;

            public int Count => m_Data.Length;

            public T this[int index]
            {
                get => m_Data[index];
                set => m_Data[index] = value;
            }

            public Line(int count)
            {
                m_Data = new T[count];
            }

            public IEnumerator<T> GetEnumerator()
            {
                switch (m_ItType)
                {
                    case XItType.Forward: return new LineForwardIt(m_Data);
                    case XItType.Backward: return new LineForwardIt(m_Data);
                    default: return default;
                }
            }

            public void SetIt(XItType type)
            {
                m_ItType = type;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < Count; i++)
                {
                    sb.Append(m_Data[i]);
                    if (i < Count - 1)
                        sb.Append(',');
                }

                return sb.ToString();
            }
        }
    }
}


using System.Collections.Generic;

namespace XFrame.Collections
{
    public partial class Csv
    {
        public class Line : IXEnumerable<string>
        {
            private XItType m_ItType;
            private string[] m_Data;

            public int Count => m_Data.Length;

            public string this[int index]
            {
                get => m_Data[index];
                set => m_Data[index] = value;
            }

            public Line(int count)
            {
                m_Data = new string[count];
            }

            public IEnumerator<string> GetEnumerator()
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
        }
    }
}

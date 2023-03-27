using System.Text;
using System.Collections.Generic;

namespace XFrame.Collections
{
    public partial class Csv<T>
    {
        /// <summary>
        /// 行数据类
        /// </summary>
        public class Line : IXEnumerable<T>
        {
            #region Inner Fields
            private XItType m_ItType;
            private T[] m_Data;
            #endregion

            #region Constructor
            /// <summary>
            /// 构造一个 <paramref name="count"/> 列的行数据
            /// </summary>
            /// <param name="count">列数</param>
            public Line(int count)
            {
                m_Data = new T[count];
            }
            #endregion

            #region Interface
            /// <summary>
            /// 列数
            /// </summary>
            public int Count => m_Data.Length;

            /// <summary>
            /// 获取或设置数据项
            /// </summary>
            /// <param name="index">索引</param>
            /// <returns>数据</returns>
            public T this[int index]
            {
                get => m_Data[index];
                set => m_Data[index] = value;
            }
            #endregion

            #region IXEnumerable Interface
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
            #endregion

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

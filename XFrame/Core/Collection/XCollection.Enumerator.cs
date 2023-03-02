using System.Collections;
using System.Collections.Generic;

namespace XFrame.Collections
{
    public partial class XCollection<T>
    {
        private struct Enumerator : IEnumerator<T>
        {
            private IEnumerator<XLinkNode<T>> m_It;

            public Enumerator(XLinkList<T> list)
            {
                m_It = list.GetEnumerator();
            }

            /// <summary>
            /// 当前迭代到的元素
            /// </summary>
            public T Current => m_It.Current.Value;

            object IEnumerator.Current => m_It.Current.Value;

            /// <summary>
            /// 释放
            /// </summary>
            public void Dispose()
            {
                m_It.Dispose();
            }

            /// <summary>
            /// 迭代下一个
            /// </summary>
            /// <returns>是否还有下一个元素</returns>
            public bool MoveNext()
            {
                return m_It.MoveNext();
            }

            /// <summary>
            /// 重置迭代器
            /// </summary>
            public void Reset()
            {
                m_It.Reset();
            }
        }
    }
}

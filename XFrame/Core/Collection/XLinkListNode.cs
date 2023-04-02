using XFrame.Modules.Pools;

namespace XFrame.Collections
{
    /// <summary>
    /// ˫������ڵ�
    /// </summary>
    /// <typeparam name="T">�洢��������</typeparam>
    public class XLinkNode<T> : IPoolObject
    {
        #region Inner Fields
        internal XLinkList<T> m_List;
        #endregion

        #region Interface
        /// <summary>
        /// ǰһ���ڵ㣬�����ǰ��ͷ�ڵ㣬��Ϊnull
        /// </summary>
        public XLinkNode<T> Pre { get; internal set; }

        /// <summary>
        /// ��һ���ڵ㣬�����ǰ��β�ڵ㣬��Ϊnull
        /// </summary>
        public XLinkNode<T> Next { get; internal set; }

        /// <summary>
        /// ����
        /// </summary>
        public T Value { get; internal set; }

        /// <summary>
        /// ɾ����ǰ�ڵ㣬���ͷŽڵ㵽����
        /// </summary>
        public void Delete()
        {
            if (Pre != null)
                Pre.Next = Next;
            else
                m_List.First = Next;

            if (Next != null)
                Next.Pre = Pre;
            else
                m_List.Last = Pre;

            m_List.Count--;
            m_List.NodePool?.Release(this);
            Value = default;
            m_List = default;
        }

        /// <summary>
        /// �ڴ˽ڵ�֮ǰ���һ��Ԫ��
        /// </summary>
        /// <param name="value">����ӵ�Ԫ��</param>
        /// <returns>Ԫ�ؽڵ�</returns>
        public XLinkNode<T> AddBefore(T value)
        {
            XLinkNode<T> node = new XLinkNode<T>();
            node.Value = value;
            node.m_List = m_List;

            node.Next = this;
            node.Pre = Pre;
            if (Pre != null)
                Pre.Next = node;
            else
                m_List.First = node;
            Pre = node;

            m_List.Count++;
            return node;
        }

        /// <summary>
        /// �ڴ˽ڵ�֮�����һ��Ԫ��
        /// </summary>
        /// <param name="value">Ԫ��</param>
        /// <returns>Ԫ�ؽڵ�</returns>
        public XLinkNode<T> AddAfter(T value)
        {
            XLinkNode<T> node = new XLinkNode<T>();
            node.Value = value;
            node.m_List = m_List;

            node.Next = Next;
            node.Pre = this;
            if (Next != null)
                Next.Pre = node;
            else
                m_List.Last = node;
            Next = node;

            m_List.Count++;
            return node;
        }
        #endregion

        #region Pool Life Fun
        void IPoolObject.OnCreate()
        {
            m_List = null;
            Pre = null;
            Next = null;
            Value = default;
        }

        void IPoolObject.OnRelease()
        {
            m_List = null;
            Pre = null;
            Next = null;
            Value = default;
        }

        void IPoolObject.OnDelete()
        {

        }
        #endregion
    }
}
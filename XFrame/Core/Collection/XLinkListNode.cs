using XFrame.Modules.Pools;

namespace XFrame.Collections
{
    /// <summary>
    /// 双向链表节点
    /// </summary>
    /// <typeparam name="T">存储数据类型</typeparam>
    public class XLinkNode<T> : IPoolObject
    {
        #region Inner Fields
        internal XLinkList<T> m_List;
        #endregion

        #region Interface
        /// <summary>
        /// 前一个节点，如果当前是头节点，则为null
        /// </summary>
        public XLinkNode<T> Pre { get; internal set; }

        /// <summary>
        /// 后一个节点，如果当前是尾节点，则为null
        /// </summary>
        public XLinkNode<T> Next { get; internal set; }

        /// <summary>
        /// 数据
        /// </summary>
        public T Value { get; internal set; }

        /// <summary>
        /// 删除当前节点，并释放节点到池中
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
        /// 在此节点之前添加一个元素
        /// </summary>
        /// <param name="value">待添加的元素</param>
        /// <returns>元素节点</returns>
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
        /// 在此节点之后添加一个元素
        /// </summary>
        /// <param name="value">元素</param>
        /// <returns>元素节点</returns>
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
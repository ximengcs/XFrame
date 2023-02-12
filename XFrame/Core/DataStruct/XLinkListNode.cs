using XFrame.Modules.Pools;

namespace XFrame.Collections
{
    /// <summary>
    /// 双向链表节点
    /// </summary>
    /// <typeparam name="T">存储数据类型</typeparam>
    public class XLinkNode<T> : IPoolObject
    {
        internal XLinkList<T> m_List;

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
            m_List.NodePool.Release(this);
            Value = default;
            m_List = default;
        }

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

        void IPoolObject.OnDestroyForever()
        {

        }
        #endregion
    }
}
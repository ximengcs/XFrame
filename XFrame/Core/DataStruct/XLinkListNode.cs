using XFrame.Modules.Pools;

namespace XFrame.Collections
{
    /// <summary>
    /// ˫������ڵ�
    /// </summary>
    /// <typeparam name="T">�洢��������</typeparam>
    public class XLinkNode<T> : IPoolObject
    {
        internal XLinkList<T> m_List;

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
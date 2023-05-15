using System.Collections.Generic;
using XFrame.Modules.Pools;

namespace XFrame.Collections
{
    /// <summary>
    /// 双向链表
    /// 注意点：	1.链表使用对象池，需要调用者使用完之后调用Dispose释放池
    ///			2.链表可以调用Node节点的Delete直接删除节点
    ///	使用场景：需要顺序迭代，需要随时删除节点，不需要随机访问
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class XLinkList<T> : IPoolObject, IXEnumerable<XLinkNode<T>>
    {
        #region Inner Fields
        private XLinkNode<T> m_First;
        private XLinkNode<T> m_Last;
        private int m_Count;
        private XItType m_ItType;

        internal bool UsePool { get; }
        #endregion

        #region Constructor
        public XLinkList() : this(false)
        {
        }

        /// <summary>
        /// 构造一个链表
        /// usePool 内部使用一个所给容量的对象池
        /// </summary>
        public XLinkList(bool usePool = true)
        {
            UsePool = usePool;
            InnerInitState();
        }

        /// <summary>
        /// 添加一个数据到链表尾
        /// </summary>
        /// <param name="data">要添加的数据</param>
        /// <returns>添加的节点</returns>
        public XLinkNode<T> AddLast(T data)
        {
            XLinkNode<T> node = UsePool ? References.Require<XLinkNode<T>>() : new XLinkNode<T>();
            node.m_List = this;
            node.Value = data;

            if (m_First == null)
            {
                m_First = node;
                m_Last = node;
                node.Pre = null;
                node.Next = null;
            }
            else
            {
                m_Last.Next = node;
                node.Pre = m_Last;
                node.Next = null;
                m_Last = node;
            }
            m_Count++;
            return node;
        }
        #endregion

        #region Interface
        /// <summary>
        /// 链表第一个节点
        /// </summary>
        public XLinkNode<T> First
        {
            get { return m_First; }
            internal set { m_First = value; }
        }

        /// <summary>
        /// 链表最后一个节点
        /// </summary>
        public XLinkNode<T> Last
        {
            get { return m_Last; }
            internal set { m_Last = value; }
        }

        /// <summary>
        /// 链表节点数量
        /// </summary>
        public int Count
        {
            get { return m_Count; }
            internal set { m_Count = value; }
        }

        /// <summary>
        /// 是否空
        /// </summary>
        public bool Empty => m_Count == 0;

        /// <summary>
        /// 尾部插入一个节点
        /// </summary>
        /// <param name="node">节点</param>
        public void AddLast(XLinkNode<T> node)
        {
            node.m_List = this;
            if (m_First == null)
            {
                m_First = node;
                m_Last = node;
                node.Pre = null;
                node.Next = null;
            }
            else
            {
                m_Last.Next = node;
                node.Pre = m_Last;
                node.Next = null;
                m_Last = node;
            }
            m_Count++;
        }

        /// <summary>
        /// 移除第一个节点
        /// </summary>
        /// <returns>移除掉的数据</returns>
        public T RemoveFirst()
        {
            if (m_First == null)
                return default;

            T value = m_First.Value;
            if (m_Count == 1)
            {
                m_First = null;
                m_Last = null;
            }
            else
            {
                m_First = m_First.Next;
                m_First.Pre = null;
            }

            m_Count--;
            return value;
        }

        /// <summary>
        /// 移除第一个节点
        /// </summary>
        /// <returns>移除的节点</returns>
        public XLinkNode<T> RemoveFirstNode()
        {
            if (m_First == null)
                return default;

            XLinkNode<T> node = m_First;
            if (m_Count == 1)
            {
                m_First = null;
                m_Last = null;
            }
            else
            {
                m_First = m_First.Next;
                m_First.Pre = null;
            }

            m_Count--;
            return node;
        }

        /// <summary>
        /// 添加一个数据到链表头
        /// </summary>
        /// <param name="data">要添加的数据</param>
        /// <returns>添加的节点</returns>
        public XLinkNode<T> AddFirst(T data)
        {
            XLinkNode<T> node = UsePool ? References.Require<XLinkNode<T>>() : new XLinkNode<T>();
            node.m_List = this;
            node.Value = data;

            if (m_First == null)
            {
                m_First = node;
                m_Last = node;
                node.Pre = null;
                node.Next = null;
            }
            else
            {
                m_First.Pre = node;
                node.Next = m_First;
                node.Pre = null;
                m_First = node;
            }
            m_Count++;
            return node;
        }

        /// <summary>
        /// 在一个节点之前插入一个节点
        /// </summary>
        /// <param name="node"></param>
        public void AddFirst(XLinkNode<T> node)
        {
            node.m_List = this;

            if (m_First == null)
            {
                m_First = node;
                m_Last = node;
                node.Next = null;
                node.Pre = null;
            }
            else
            {
                m_First.Pre = node;
                node.Next = m_First;
                node.Pre = null;
                m_First = node;
            }
            m_Count++;
        }

        /// <summary>
        /// 移除最后一个元素
        /// </summary>
        /// <returns>移除的元素</returns>
        public T RemoveLast()
        {
            if (m_Last == null)
                return default;

            T value = m_Last.Value;
            if (m_Count == 1)
            {
                m_First = null;
                m_Last = null;
            }
            else
            {
                m_Last = m_Last.Pre;
                m_Last.Next = null;
            }

            m_Count--;
            return value;
        }

        /// <summary>
        /// 移除最后一个节点
        /// </summary>
        /// <returns>移除的节点</returns>
        public XLinkNode<T> RemoveLastNode()
        {
            if (m_Last == null)
                return default;

            XLinkNode<T> node = m_Last;
            if (m_Count == 1)
            {
                m_First = null;
                m_Last = null;
            }
            else
            {
                m_Last = m_Last.Pre;
                m_Last.Next = null;
            }

            m_Count--;
            return node;
        }

        /// <summary>
        /// 移除一个元素
        /// </summary>
        /// <param name="value">待移除的元素</param>
        public void Remove(T value)
        {
            foreach (XLinkNode<T> node in this)
            {
                if (node.Value.Equals(value))
                {
                    node.Delete();
                    return;
                }
            }
        }

        /// <summary>
        /// 清除链表
        /// </summary>
        public void Clear()
        {
            if (UsePool)
            {
                foreach (XLinkNode<T> node in this)
                    References.Release(node);
            }
            InnerInitState();
        }
        #endregion

        #region IXEnumerable Interface
        public void SetIt(XItType type)
        {
            m_ItType = type;
        }

        public IEnumerator<XLinkNode<T>> GetEnumerator()
        {
            switch (m_ItType)
            {
                case XItType.Forward: return new ForwardIt(this);
                case XItType.Backward: return new BackwardIt(this);
                default: return default;
            }
        }
        #endregion

        #region Pool Life Fun
        void IPoolObject.OnCreate()
        {

        }

        void IPoolObject.OnRequest()
        {
            InnerInitState();
        }

        void IPoolObject.OnRelease()
        {
            InnerInitState();
        }

        void IPoolObject.OnDelete()
        {

        }
        #endregion

        private void InnerInitState()
        {
            m_ItType = XItType.Forward;
            m_First = null;
            m_Last = null;
            m_Count = 0;
        }
    }

}

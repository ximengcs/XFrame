using System.Collections.Generic;
using XFrame.Modules.Pools;

namespace XFrame.Collections
{
    /// <summary>
    /// 双向链表
    /// </summary>
    /// <typeparam name="T">持有类型</typeparam>
    public partial class XLinkList<T> : IPoolObject, IXEnumerable<XLinkNode<T>>, IXEnumerable<T>
    {
        #region Inner Fields
        private XLinkNode<T> m_First;
        private XLinkNode<T> m_Last;
        private int m_Count;
        private XItType m_ItType;

        internal bool UsePool { get; }
        #endregion

        #region Constructor
        /// <summary>
        /// 构造一个双向链表, 不使用对象池
        /// </summary>
        public XLinkList() : this(false)
        {
        }

        /// <summary>
        /// 构造一个双向链表
        /// </summary>
        /// <param name="usePool">是否使用对象池</param>
        public XLinkList(bool usePool = true)
        {
            UsePool = usePool;
            InnerInitState();
        }

        /// <summary>
        /// 添加元素到尾部
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
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
        /// 首个元素
        /// </summary>
        public XLinkNode<T> First
        {
            get { return m_First; }
            internal set { m_First = value; }
        }

        /// <summary>
        /// 最后一个元素
        /// </summary>
        public XLinkNode<T> Last
        {
            get { return m_Last; }
            internal set { m_Last = value; }
        }

        /// <summary>
        /// 元素数量
        /// </summary>
        public int Count
        {
            get { return m_Count; }
            internal set { m_Count = value; }
        }

        /// <summary>
        /// 列表是否为空
        /// </summary>
        public bool Empty => m_Count == 0;

        /// <summary>
        /// 在列表尾添加一个节点
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
        /// 移除第一个元素
        /// </summary>
        /// <returns>移除的元素</returns>
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
        /// 在表头添加一个元素
        /// </summary>
        /// <param name="data">元素</param>
        /// <returns>节点</returns>
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
        /// 在表头添加一个节点
        /// </summary>
        /// <param name="node">添加的节点</param>
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
        /// 移除元素
        /// </summary>
        /// <param name="value">元素</param>
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
        /// 清除元素
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
        /// <inheritdoc/>
        public void SetIt(XItType type)
        {
            m_ItType = type;
        }

        /// <inheritdoc/>
        public IEnumerator<XLinkNode<T>> GetEnumerator()
        {
            switch (m_ItType)
            {
                case XItType.Forward: return new ForwardIt(this);
                case XItType.Backward: return new BackwardIt(this);
                default: return default;
            }
        }

        IEnumerator<T> IXEnumerable<T>.GetEnumerator()
        {
            switch (m_ItType)
            {
                case XItType.Forward: return new ElementForwardIt(this);
                case XItType.Backward: return new ElementBackwardIt(this);
                default: return default;
            }
        }
        #endregion

        #region Pool Life Fun
        /// <inheritdoc/>
        public string MarkName { get; set; }

        IPool IPoolObject.InPool { get; set; }

        int IPoolObject.PoolKey => 0;

        void IPoolObject.OnCreate()
        {

        }

        void IPoolObject.OnRequest()
        {
            InnerInitState();
        }

        void IPoolObject.OnRelease()
        {
            Clear();
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

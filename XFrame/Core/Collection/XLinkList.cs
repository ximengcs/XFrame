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
    public partial class XLinkList<T> : IXEnumerable<XLinkNode<T>>
    {
        private IPool<XLinkNode<T>> m_NodePool;
        private XLinkNode<T> m_First;
        private XLinkNode<T> m_Last;
        private int m_Count;
        private XItType m_ItType;

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
            internal set { m_First = value; }
        }

        /// <summary>
        /// 链表节点数量
        /// </summary>
        public int Count
        {
            get { return m_Count; }
            internal set { m_Count = value; }
        }

        public bool Empty => m_Count == 0;

        internal IPool<XLinkNode<T>> NodePool => m_NodePool;

        /// <summary>
        /// 构造一个链表
        /// usePool 内部使用一个所给容量的对象池
        /// </summary>
        public XLinkList(bool usePool = true)
        {
            if (usePool)
                m_NodePool = PoolModule.Inst.GetOrNew<XLinkNode<T>>();
            m_ItType = XItType.Forward;
            m_First = null;
            m_Last = null;
            m_Count = 0;
        }

        /// <summary>
        /// 添加一个数据到链表尾
        /// </summary>
        /// <param name="data">要添加的数据</param>
        /// <returns>添加的节点</returns>
        public XLinkNode<T> AddLast(T data)
        {
            XLinkNode<T> node;
            if (m_NodePool != null)
                m_NodePool.Require(out node);
            else
                node = new XLinkNode<T>();
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
            XLinkNode<T> node;
            if (m_NodePool != null)
                m_NodePool.Require(out node);
            else
                node = new XLinkNode<T>();
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

        public void Clear()
        {
            if (m_NodePool != null)
            {
                XLinkNode<T> node = m_First;
                while (node != null)
                    m_NodePool.Release(node);
            }

            m_First = null;
            m_Last = null;
            m_Count = 0;
        }

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

        /// <summary>
        /// 释放Node池
        /// </summary>
        public void Dispose()
        {
            m_NodePool?.Dispose();
        }
    }

}

using System;
using System.Collections.Generic;
using XFrame.Modules.Pools;

namespace XFrame.Collections
{
    /// <summary>
    /// 双向链表
    /// 注意点：	1.链表使用对象池，需要调用者使用完之后调用Dispose释放池
    ///			2.链表可以调用Node节点的Remove直接删除节点
    ///	使用场景：需要顺序迭代，需要随时删除节点，不需要随机访问
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class XLinkList<T> : IXEnumerable<T>, IUsePool
    {
        private const int DEFAULT_POOL = 16;
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

        internal IPool<XLinkNode<T>> NodePool => m_NodePool;

        /// <summary>
        /// 构造一个链表
        /// 内部使用一个所给容量的对象池
        /// </summary>
        /// <param name="poolCapacity">节点对象池容量</param>
        public XLinkList(int poolCapacity)
        {
            m_NodePool = PoolModule.Inst.GetOrNew<XLinkNode<T>>()
                .Require<XLinkNode<T>>(poolCapacity);
            m_ItType = XItType.Forward;
            m_First = null;
            m_Last = null;
            m_Count = 0;
        }

        /// <summary>
        /// 构造一个链表
        /// 内部使用一个默认容量的对象池
        /// </summary>
        public XLinkList(bool usePool = true)
        {
            if (usePool)
            {
                m_NodePool = PoolModule.Inst.GetOrNew<XLinkNode<T>>()
                .Require<XLinkNode<T>>(DEFAULT_POOL);
            }

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

        public IEnumerator<T> GetEnumerator()
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
            if (m_NodePool != null)
            {
                PoolModule.Inst.GetOrNew<XLinkNode<T>>()
                .Release(m_NodePool);
                m_NodePool = null;
            }
        }
    }

}

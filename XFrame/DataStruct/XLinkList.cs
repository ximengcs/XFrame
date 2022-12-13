using System;
using XFrame.Modules;

namespace XFrame.Collections
{
    /// <summary>
    /// 双向链表
    /// 注意点：	1.链表使用对象池，需要调用者使用完之后调用Dispose释放池
    ///			2.链表可以调用Node节点的Remove直接删除节点
    ///	使用场景：需要顺序迭代，需要随时删除节点，不需要随机访问
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XLinkList<T> : IDisposable
    {
        private const int DEFAULT_POOL = 16;
        private IPool<XLinkNode<T>> m_NodePool;
        private XLinkNode<T> m_First;
        private XLinkNode<T> m_Last;
        private int m_Count;

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
                .Require(poolCapacity);
            m_First = null;
            m_Last = null;
        }

        /// <summary>
        /// 构造一个链表
        /// 内部使用一个默认容量的对象池
        /// </summary>
        public XLinkList()
        {
            m_NodePool = PoolModule.Inst.GetOrNew<XLinkNode<T>>()
                .Require(DEFAULT_POOL);
            m_First = null;
            m_Last = null;
        }

        /// <summary>
        /// 添加一个数据到链表尾
        /// </summary>
        /// <param name="data">要添加的数据</param>
        /// <returns>添加的节点</returns>
        public XLinkNode<T> AddLast(T data)
        {
            m_NodePool.Require(out XLinkNode<T> node);
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
            m_NodePool.Require(out XLinkNode<T> node);
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

        /// <summary>
        /// 释放链表
        /// </summary>
        public void Dispose()
        {
            PoolModule.Inst.GetOrNew<XLinkNode<T>>()
                .Release(m_NodePool);
            m_NodePool = null;
        }
    }

}

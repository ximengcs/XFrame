using System;
using System.Collections.Generic;
using XFrame.Modules.Pools;

namespace XFrame.Collections
{
    /// <summary>
    /// ˫������
    /// ע��㣺	1.����ʹ�ö���أ���Ҫ������ʹ����֮�����Dispose�ͷų�
    ///			2.������Ե���Node�ڵ��Removeֱ��ɾ���ڵ�
    ///	ʹ�ó�������Ҫ˳���������Ҫ��ʱɾ���ڵ㣬����Ҫ�������
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
        /// �����һ���ڵ�
        /// </summary>
        public XLinkNode<T> First
        {
            get { return m_First; }
            internal set { m_First = value; }
        }

        /// <summary>
        /// �������һ���ڵ�
        /// </summary>
        public XLinkNode<T> Last
        {
            get { return m_Last; }
            internal set { m_First = value; }
        }

        /// <summary>
        /// ����ڵ�����
        /// </summary>
        public int Count
        {
            get { return m_Count; }
            internal set { m_Count = value; }
        }

        internal IPool<XLinkNode<T>> NodePool => m_NodePool;

        /// <summary>
        /// ����һ������
        /// �ڲ�ʹ��һ�����������Ķ����
        /// </summary>
        /// <param name="poolCapacity">�ڵ���������</param>
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
        /// ����һ������
        /// �ڲ�ʹ��һ��Ĭ�������Ķ����
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
        /// ���һ�����ݵ�����β
        /// </summary>
        /// <param name="data">Ҫ��ӵ�����</param>
        /// <returns>��ӵĽڵ�</returns>
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
        /// ���һ�����ݵ�����ͷ
        /// </summary>
        /// <param name="data">Ҫ��ӵ�����</param>
        /// <returns>��ӵĽڵ�</returns>
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
        /// �ͷ�Node��
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

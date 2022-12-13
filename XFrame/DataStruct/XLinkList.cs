using System;
using XFrame.Modules;

namespace XFrame.Collections
{
    /// <summary>
    /// ˫������
    /// ע��㣺	1.����ʹ�ö���أ���Ҫ������ʹ����֮�����Dispose�ͷų�
    ///			2.������Ե���Node�ڵ��Removeֱ��ɾ���ڵ�
    ///	ʹ�ó�������Ҫ˳���������Ҫ��ʱɾ���ڵ㣬����Ҫ�������
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
                .Require(poolCapacity);
            m_First = null;
            m_Last = null;
        }

        /// <summary>
        /// ����һ������
        /// �ڲ�ʹ��һ��Ĭ�������Ķ����
        /// </summary>
        public XLinkList()
        {
            m_NodePool = PoolModule.Inst.GetOrNew<XLinkNode<T>>()
                .Require(DEFAULT_POOL);
            m_First = null;
            m_Last = null;
        }

        /// <summary>
        /// ���һ�����ݵ�����β
        /// </summary>
        /// <param name="data">Ҫ��ӵ�����</param>
        /// <returns>��ӵĽڵ�</returns>
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
        /// ���һ�����ݵ�����ͷ
        /// </summary>
        /// <param name="data">Ҫ��ӵ�����</param>
        /// <returns>��ӵĽڵ�</returns>
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
        /// �ͷ�����
        /// </summary>
        public void Dispose()
        {
            PoolModule.Inst.GetOrNew<XLinkNode<T>>()
                .Release(m_NodePool);
            m_NodePool = null;
        }
    }

}

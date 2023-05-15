using System.Collections.Generic;
using XFrame.Modules.Pools;

namespace XFrame.Collections
{
    /// <summary>
    /// ˫������
    /// ע��㣺	1.����ʹ�ö���أ���Ҫ������ʹ����֮�����Dispose�ͷų�
    ///			2.������Ե���Node�ڵ��Deleteֱ��ɾ���ڵ�
    ///	ʹ�ó�������Ҫ˳���������Ҫ��ʱɾ���ڵ㣬����Ҫ�������
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
        /// ����һ������
        /// usePool �ڲ�ʹ��һ�����������Ķ����
        /// </summary>
        public XLinkList(bool usePool = true)
        {
            UsePool = usePool;
            InnerInitState();
        }

        /// <summary>
        /// ���һ�����ݵ�����β
        /// </summary>
        /// <param name="data">Ҫ��ӵ�����</param>
        /// <returns>��ӵĽڵ�</returns>
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
            internal set { m_Last = value; }
        }

        /// <summary>
        /// ����ڵ�����
        /// </summary>
        public int Count
        {
            get { return m_Count; }
            internal set { m_Count = value; }
        }

        /// <summary>
        /// �Ƿ��
        /// </summary>
        public bool Empty => m_Count == 0;

        /// <summary>
        /// β������һ���ڵ�
        /// </summary>
        /// <param name="node">�ڵ�</param>
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
        /// �Ƴ���һ���ڵ�
        /// </summary>
        /// <returns>�Ƴ���������</returns>
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
        /// �Ƴ���һ���ڵ�
        /// </summary>
        /// <returns>�Ƴ��Ľڵ�</returns>
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
        /// ���һ�����ݵ�����ͷ
        /// </summary>
        /// <param name="data">Ҫ��ӵ�����</param>
        /// <returns>��ӵĽڵ�</returns>
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
        /// ��һ���ڵ�֮ǰ����һ���ڵ�
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
        /// �Ƴ����һ��Ԫ��
        /// </summary>
        /// <returns>�Ƴ���Ԫ��</returns>
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
        /// �Ƴ����һ���ڵ�
        /// </summary>
        /// <returns>�Ƴ��Ľڵ�</returns>
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
        /// �Ƴ�һ��Ԫ��
        /// </summary>
        /// <param name="value">���Ƴ���Ԫ��</param>
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
        /// �������
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

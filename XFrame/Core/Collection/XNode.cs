using System;
using System.Collections.Generic;

namespace XFrame.Collections
{
    /// <summary>
    /// 节点数据类型
    /// </summary>
    /// <typeparam name="T">持有类型</typeparam>
    public partial class XNode<T> : IXEnumerable<XNode<T>>
    {
        #region Inner Fields
        private XItType m_ItType;
        private List<XNode<T>> m_List;
        #endregion

        #region Constructor
        /// <summary>
        /// 构造一个节点
        /// </summary>
        public XNode()
        {
            Level = 0;
            m_List = new List<XNode<T>>();
        }
        #endregion

        #region Interface
        /// <summary>
        /// 持有数据
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// 处于层级
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// 添加一个孩子节点
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>节点</returns>
        public XNode<T> Add(T value)
        {
            XNode<T> node = new XNode<T>();
            node.Value = value;
            node.Level = Level + 1;
            m_List.Add(node);
            return node;
        }

        /// <summary>
        /// 在孩子节点中匹配满足条件 <paramref name="condition"/> 的节点并添加节点
        /// </summary>
        /// <param name="condition">需要满足的条件</param>
        /// <param name="value">数据</param>
        /// <returns>节点</returns>
        public XNode<T> Add(Func<XNode<T>, bool> condition, T value)
        {
            XNode<T> node = InnerFind(condition);
            if (node != null)
                return node.Add(value);
            else
                return Add(value);
        }

        /// <summary>
        /// 获取一个满足 <paramref name="condition"/> 条件的节点
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>节点</returns>
        public XNode<T> Get(Func<XNode<T>, bool> condition)
        {
            return InnerFind(condition);
        }

        /// <summary>
        /// 递归地迭代所有孩子节点
        /// </summary>
        /// <param name="callback">处理委托</param>
        public void ForEachAll(Func<XNode<T>, bool> callback)
        {
            InnerForEachAll(callback);
        }
        #endregion

        #region IXEnumerable Interface
        public IEnumerator<XNode<T>> GetEnumerator()
        {
            switch (m_ItType)
            {
                case XItType.Forward: return new ListExt.ForwardIt<XNode<T>>(m_List);
                case XItType.Backward: return new ListExt.BackwardIt<XNode<T>>(m_List);
                default: return default;
            }
        }

        public void SetIt(XItType type)
        {
            m_ItType = type;
        }
        #endregion

        #region Inner Implement
        private bool InnerForEachAll(Func<XNode<T>, bool> callback)
        {
            foreach (XNode<T> node in m_List)
            {
                if (callback(node))
                {
                    if (node.InnerForEachAll(callback))
                        return true;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        private XNode<T> InnerFind(Func<XNode<T>, bool> condition)
        {
            foreach (XNode<T> node in m_List)
            {
                if (condition(node))
                {
                    return node;
                }
                else
                {
                    XNode<T> child = node.InnerFind(condition);
                    if (child != null)
                        return child;
                }
            }
            return default;
        }
        #endregion
    }
}

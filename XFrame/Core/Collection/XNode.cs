using System;
using System.Collections;
using System.Collections.Generic;

namespace XFrame.Collections
{
    public class XNode<T> : IEnumerable<XNode<T>>
    {
        private List<XNode<T>> m_List;

        public T Value { get; set; }

        public XNode()
        {
            m_List = new List<XNode<T>>();
        }

        public XNode<T> Add(T value)
        {
            XNode<T> node = new XNode<T>();
            node.Value = value;
            m_List.Add(node);
            return node;
        }

        public XNode<T> Add(Func<XNode<T>, bool> condition, T value)
        {
            XNode<T> node = InnerFind(condition);
            if (node != null)
                return node.Add(value);
            else
                return Add(value);
        }

        public XNode<T> Get(Func<XNode<T>, bool> condition)
        {
            return InnerFind(condition);
        }

        public void ForEachAll(Func<XNode<T>, bool> callback)
        {
            InnerForEachAll(callback);
        }

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

        public IEnumerator<XNode<T>> GetEnumerator()
        {
            return m_List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_List.GetEnumerator();
        }
    }
}

using System;
using XFrame.Collections;
using XFrame.Modules.Pools;
using XFrame.Modules.XType;

namespace XFrame.Core
{
    public class ArrayParser<T> : IParser<XLinkList<T>> where T : IParser
    {
        private const char SPLIT = ',';
        private char m_Split;
        private string m_Origin;

        public int Count => Value != null ? Value.Count : 0;
        public bool Empty => Value != null ? Value.Count == 0 : true;
        public XLinkList<T> Value { get; private set; }

        object IParser.Value => Value;

        int IPoolObject.PoolKey => default;
        IPool IPoolObject.InPool { get; set; }

        public char Split
        {
            get => m_Split;
            set
            {
                if (m_Split != value)
                {
                    m_Split = value;
                    Parse(m_Origin);
                }
            }
        }

        public ArrayParser()
        {
            m_Split = SPLIT;
        }

        public ArrayParser(char splitchar)
        {
            m_Split = splitchar;
        }

        public void Release()
        {
            References.Release(this);
        }

        public XLinkList<T> Parse(string pattern)
        {
            m_Origin = pattern;
            if (Value == null)
                Value = new XLinkList<T>();
            else
                Value.Clear();

            if (!string.IsNullOrEmpty(pattern))
            {
                string[] pArray = pattern.Split(m_Split);
                Type type = typeof(T);
                for (int i = 0; i < pArray.Length; i++)
                {
                    T parser = (T)References.Require(type);
                    parser.Parse(pArray[i]);
                    Value.AddLast(parser);
                }
            }

            return Value;
        }

        public int IndexOf(object value)
        {
            if (Value == null)
                return -1;
            int index = 0;
            foreach (XLinkNode<T> node in Value)
            {
                object other = node.Value;
                if (other != null && other.Equals(value))
                    return index;
                index++;
            }
            return -1;
        }

        public bool Has(object value)
        {
            return IndexOf(value) != -1;
        }

        public T Get(int index)
        {
            if (Value == null)
                return default;
            int current = 0;
            foreach (XLinkNode<T> node in Value)
            {
                if (index == current)
                    return node.Value;
                current++;
            }
            return default;
        }

        public int IndexOf(object value, Func<object, object, bool> action)
        {
            if (Value == null)
                return -1;
            int index = 0;
            foreach (XLinkNode<T> node in Value)
            {
                object other = node.Value.Value;
                if (other != null && action(value, other))
                    return index;
                index++;
            }
            return -1;
        }

        public bool Has(object value, Func<object, object, bool> action)
        {
            return IndexOf(value, action) != -1;
        }

        object IParser.Parse(string pattern)
        {
            return Parse(pattern);
        }

        public override string ToString()
        {
            return m_Origin;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (Value == null)
            {
                if (obj != null)
                    return false;
                else
                    return true;
            }
            foreach (XLinkNode<T> v in Value)
            {
                if (!v.Equals(obj))
                    return false;
            }
            return true;
        }

        void IPoolObject.OnCreate()
        {

        }

        void IPoolObject.OnRequest()
        {
            m_Split = SPLIT;
        }

        void IPoolObject.OnRelease()
        {
            foreach (XLinkNode<T> v in Value)
                References.Release(v.Value);
            Value.Clear();
        }

        void IPoolObject.OnDelete()
        {
            Value.Clear();
            Value = null;
        }
    }
}

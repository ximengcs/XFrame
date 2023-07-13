using System;
using System.Text;
using XFrame.Collections;
using XFrame.Modules.Pools;
using XFrame.Modules.XType;

namespace XFrame.Core
{
    public class ArrayParser<T> : IParser<XLinkList<T>> where T : IParser
    {
        private const char SPLIT = ',';
        private char m_Split;

        public XLinkList<T> Value { get; private set; }

        object IParser.Value => Value;

        int IPoolObject.PoolKey => default;

        public ArrayParser()
        {
            m_Split = SPLIT;
        }

        public ArrayParser(char splitchar)
        {
            m_Split = splitchar;
        }

        public XLinkList<T> Parse(string pattern)
        {
            Value = new XLinkList<T>();
            if (!string.IsNullOrEmpty(pattern))
            {
                string[] pArray = pattern.Split(m_Split);
                Type type = typeof(T);
                for (int i = 0; i < pArray.Length; i++)
                {
                    T parser = (T)TypeModule.Inst.CreateInstance(type);
                    parser.Parse(pArray[i]);
                    Value.AddLast(parser);
                }
            }

            return Value;
        }

        public int IndexOf(object value)
        {
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
            StringBuilder sb = new StringBuilder();
            foreach (XLinkNode<T> v in Value)
            {
                sb.Append(v.Value);
                if (v.Next != null)
                    sb.Append(SPLIT);
            }
            return sb.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
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
            Value.Clear();
        }

        void IPoolObject.OnDelete()
        {
            Value.Clear();
            Value = null;
        }
    }
}

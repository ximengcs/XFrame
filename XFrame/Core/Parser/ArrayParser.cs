using System;
using System.Text;
using XFrame.Collections;

namespace XFrame.Core
{
    public class ArrayParser<T> : IParser<XLinkList<T>> where T : IParser
    {
        private const char SPLIT = ',';
        private char m_Split;

        public XLinkList<T> Value { get; private set; }

        object IParser.Value => Value;

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
            string[] pArray = pattern.Split(m_Split);
            Value = new XLinkList<T>(false);
            Type type = typeof(T);
            for (int i = 0; i < pArray.Length; i++)
            {
                T parser = (T)Activator.CreateInstance(type);
                parser.Parse(pArray[i]);
                Value.AddLast(parser);
            }

            return Value;
        }

        public bool Has(object value)
        {
            foreach (XLinkNode<T> node in Value)
            {
                object other = node.Value;
                if (other != null && other.Equals(value))
                    return true;
            }
            return false;
        }

        public bool Has(object value, Func<object, object, bool> action)
        {
            foreach (XLinkNode<T> node in Value)
            {
                object other = node.Value.Value;
                if (other != null && action(value, other))
                    return true;
            }
            return false;
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
    }
}

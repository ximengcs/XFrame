using System;
using XFrame.Collections;

namespace XFrame.Core
{
    public class ArrayParser<T, VT> : IParser<XLinkList<T>> where T : IParser<VT>
    {
        private const char SPLIT = ',';
        private char m_Split;

        public XLinkList<T> Value { get; private set; }

        public char SplitChar
        {
            get { return m_Split != 0 ? m_Split : SPLIT; }
            set { m_Split = value; }
        }

        public XLinkList<T> Parse(string pattern)
        {
            string[] pArray = pattern.Split(SplitChar);
            Value = new XLinkList<T>();
            Type type = typeof(T);
            for (int i = 0; i < pArray.Length; i++)
            {
                T parser = (T)Activator.CreateInstance(type);
                parser.Parse(pArray[i]);
                Value.AddLast(parser);
            }

            return Value;
        }

        public bool Has(VT value)
        {
            foreach (XLinkNode<T> node in Value)
            {
                VT other = node.Value.Value;
                if (other != null && other.Equals(value))
                    return true;
            }
            return false;
        }
    }
}

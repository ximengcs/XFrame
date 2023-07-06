using System;
using XFrame.Modules.Pools;

namespace XFrame.Core
{
    public class PairParser<K, V> : IParser<Pair<K, V>> where K : IParser where V : IParser
    {
        private const char SPLIT = '|';
        private char m_Split;
        private IParser m_KParser;
        private IParser m_VParser;

        public PairParser()
        {
            m_Split = SPLIT;
            InnerInitParser();
        }

        public PairParser(char splitchar)
        {
            m_Split = splitchar;
            InnerInitParser();
        }

        private void InnerInitParser()
        {
            m_KParser = (IParser)Activator.CreateInstance(typeof(K));
            m_VParser = (IParser)Activator.CreateInstance(typeof(V));
            Value = new Pair<K, V>((K)m_KParser, (V)m_VParser);
        }

        public Pair<K, V> Value { get; private set; }

        object IParser.Value => Value;

        int IPoolObject.PoolKey => throw new NotImplementedException();

        public Pair<K, V> Parse(string pattern)
        {
            string[] values = pattern.Split(m_Split);
            if (values.Length == 1)
            {
                m_KParser.Parse(values[0]);
            }
            else if (values.Length == 2)
            {
                m_KParser.Parse(values[0]);
                m_VParser.Parse(values[1]);
            }
            return Value;
        }

        object IParser.Parse(string pattern)
        {
            return Parse(pattern);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            IParser parser = obj as IParser;
            return parser != null ? Value.Equals(parser.Value) : Value.Equals(obj);
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

        }

        void IPoolObject.OnDelete()
        {

        }

        public static bool operator ==(PairParser<K, V> src, object tar)
        {
            if (ReferenceEquals(src, null))
            {
                return ReferenceEquals(tar, null);
            }
            else
            {
                return src.Equals(tar);
            }
        }

        public static bool operator !=(PairParser<K, V> src, object tar)
        {
            if (ReferenceEquals(src, null))
            {
                return !ReferenceEquals(tar, null);
            }
            else
            {
                return !src.Equals(tar);
            }
        }

        public static implicit operator PairParser<K, V>(Pair<K, V> value)
        {
            PairParser<K, V> parser = new PairParser<K, V>();
            parser.Value = value;
            return parser;
        }

        public static implicit operator Pair<K, V>(PairParser<K, V> value)
        {
            return value.Value;
        }
    }
}

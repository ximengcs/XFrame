using System;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Pools;

namespace XFrame.Core
{
    public class PairParser<K, V> : IParser<Pair<K, V>>, ICanConfigLog where K : IParser where V : IParser
    {
        private const char SPLIT = '|';
        private char m_Split;
        private string m_Origin;
        private IParser m_KParser;
        private IParser m_VParser;

        public LogLevel LogLv
        {
            get => throw new NotSupportedException();
            set
            {
                ICanConfigLog configer = m_KParser as ICanConfigLog;
                if (configer != null)
                    configer.LogLv = value;
                configer = m_VParser as ICanConfigLog;
                if (configer != null)
                    configer.LogLv = value;
            }
        }

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
            m_KParser = (IParser)References.Require(typeof(K));
            m_VParser = (IParser)References.Require(typeof(V));
            Value = new Pair<K, V>((K)m_KParser, (V)m_VParser);
        }

        public Pair<K, V> Value { get; private set; }

        object IParser.Value => Value;

        int IPoolObject.PoolKey => default;
        public string MarkName { get; set; }

        IPool IPoolObject.InPool { get; set; }

        public Pair<K, V> Parse(string pattern)
        {
            m_Origin = pattern;
            if (!string.IsNullOrEmpty(pattern))
            {
                string[] values = pattern.Split(m_Split);
                if (values.Length == 1)
                {
                    m_KParser.Parse(values[0]);
                    m_VParser.Parse(null);
                }
                else if (values.Length == 2)
                {
                    m_KParser.Parse(values[0]);
                    m_VParser.Parse(values[1]);
                }
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

        public void Release()
        {
            References.Release(this);
        }

        void IPoolObject.OnCreate(IPoolModule module)
        {

        }

        void IPoolObject.OnRequest()
        {
            m_Split = SPLIT;
        }

        void IPoolObject.OnRelease()
        {
            m_KParser.OnRelease();
            m_VParser.OnRelease();
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
            PairParser<K, V> parser = References.Require<PairParser<K, V>>();
            parser.Value = value;
            return parser;
        }

        public static implicit operator Pair<K, V>(PairParser<K, V> value)
        {
            return value.Value;
        }
    }
}

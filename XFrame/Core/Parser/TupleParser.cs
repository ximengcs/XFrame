using System;
using XFrame.Modules.Pools;

namespace XFrame.Core
{
    public class TupleParser<T1> : PoolObjectBase, IParser<ValueTuple<T1>>
    {
        private readonly static char SPLIT = ';';

        private ValueTuple<T1> m_Value;

        public ValueTuple<T1> Value => m_Value;

        object IParser.Value => m_Value;

        public ValueTuple<T1> Parse(string pattern)
        {
            pattern = pattern.Substring(1, pattern.Length - 2);
            string[] contents = pattern.Split(SPLIT);
            m_Value = new ValueTuple<T1>(
                XModule.Serialize.DeserializeToObject<T1>(contents[0]));

            return m_Value;
        }

        object IParser.Parse(string pattern)
        {
            return Parse(pattern);
        }


        public override string ToString()
        {
            return m_Value.ToString();
        }

        public override int GetHashCode()
        {
            return m_Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            IParser parser = obj as IParser;
            return parser != null ? m_Value.Equals(parser.Value) : m_Value.Equals(obj);
        }

        protected override void OnRequestFromPool()
        {
            base.OnRequestFromPool();
            PoolKey = 0;
        }

        public static bool operator ==(TupleParser<T1> src, object tar)
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

        public static bool operator !=(TupleParser<T1> src, object tar)
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

        public static implicit operator TupleParser<T1>(T1 value)
        {
            TupleParser<T1> parser = new TupleParser<T1>();
            parser.m_Value.Item1 = value;
            return parser;
        }

        public static implicit operator T1(TupleParser<T1> value)
        {
            if (value == null)
                return default;
            return value.m_Value.Item1;
        }
    }

    public class TupleParser<T1, T2> : PoolObjectBase, IParser<ValueTuple<T1, T2>>
    {
        private readonly static char SPLIT = ';';

        private ValueTuple<T1, T2> m_Value;

        public ValueTuple<T1, T2> Value => m_Value;

        object IParser.Value => m_Value;

        public ValueTuple<T1, T2> Parse(string pattern)
        {
            pattern = pattern.Substring(1, pattern.Length - 2);
            string[] contents = pattern.Split(SPLIT);
            m_Value = new ValueTuple<T1, T2>(
                XModule.Serialize.DeserializeToObject<T1>(contents[0]),
                XModule.Serialize.DeserializeToObject<T2>(contents[1]));

            return m_Value;
        }

        object IParser.Parse(string pattern)
        {
            return Parse(pattern);
        }

        public override string ToString()
        {
            return m_Value.ToString();
        }

        public override int GetHashCode()
        {
            return m_Value.GetHashCode();
        }

        public void Release()
        {
            References.Release(this);
        }

        public override bool Equals(object obj)
        {
            IParser parser = obj as IParser;
            return parser != null ? m_Value.Equals(parser.Value) : m_Value.Equals(obj);
        }

        protected override void OnRequestFromPool()
        {
            base.OnRequestFromPool();
            PoolKey = 0;
        }

        public static bool operator ==(TupleParser<T1, T2> src, object tar)
        {
            return src.Equals(tar);
        }

        public static bool operator !=(TupleParser<T1, T2> src, object tar)
        {
            return !src.Equals(tar);
        }

        public static implicit operator TupleParser<T1, T2>((T1, T2) value)
        {
            TupleParser<T1, T2> parser = References.Require<TupleParser<T1, T2>>();
            parser.m_Value.Item1 = value.Item1;
            parser.m_Value.Item2 = value.Item2;
            return parser;
        }

        public static implicit operator (T1, T2)(TupleParser<T1, T2> value)
        {
            return (value.m_Value.Item1, value.m_Value.Item2);
        }
    }
}
using System;
using System.Collections.Generic;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Pools;

namespace XFrame.Core
{
    public class UniversalParser : PoolObjectBase, IParser<string>
    {
        private string m_Value;
        private int m_IntValue;
        private float m_FloatValue;
        private bool m_BoolValue;
        private Dictionary<Type, IParser> m_Parsers;

        public string Value => m_Value;
        public int IntValue => m_IntValue;
        public float FloatValue => m_FloatValue;
        public bool BoolValue => m_BoolValue;

        object IParser.Value => m_Value;

        public T GetOrAddParser<T>() where T : IParser
        {
            T parser = GetParser<T>();
            if (parser == null)
                parser = AddParser<T>();
            return parser;
        }

        public T GetParser<T>() where T : IParser
        {
            if (m_Parsers.TryGetValue(typeof(T), out IParser parser))
            {
                parser.Parse(m_Value);
                return (T)parser;
            }
            return default;
        }

        public void RemoveParser<T>() where T : IParser
        {
            Type type = typeof(T);
            if (m_Parsers.TryGetValue(type, out IParser parser))
            {
                m_Parsers.Remove(type);
                References.Release(parser);
            }
        }

        public T AddParser<T>(T parser) where T : IParser
        {
            if (parser != null)
            {
                Type type = typeof(T);
                parser.Parse(m_Value);
                if (m_Parsers.TryGetValue(type, out IParser oldParser))
                {
                    Log.Warning("XFrame", $"Universal parser already has parser {type.Name}. will release old.");
                    References.Release(oldParser);
                    m_Parsers[type] = parser;
                }
                else
                {
                    m_Parsers.Add(type, parser);
                }
            }
            return parser;
        }

        public IParser AddParser(Type parserType)
        {
            if (!m_Parsers.TryGetValue(parserType, out IParser parser))
            {
                parser = (IParser)References.Require(parserType);
                parser.Parse(m_Value);
                m_Parsers.Add(parserType, parser);
            }
            return parser;
        }

        public T AddParser<T>() where T : IParser
        {
            return (T)AddParser(typeof(T));
        }

        private UniversalParser()
        {
            m_Parsers = new Dictionary<Type, IParser>();
        }

        private void InnerInitIntValue(int value)
        {
            m_IntValue = value;
            m_Value = value.ToString();
            m_FloatValue = value;
            m_BoolValue = value == 0 ? false : true;
        }

        private void InnerInitFloatValue(float value)
        {
            m_IntValue = (int)value;
            m_Value = value.ToString();
            m_FloatValue = value;
            m_BoolValue = value == 0 ? false : true;
        }

        private void InnerInitBoolValue(bool value)
        {
            m_IntValue = value ? 1 : 0;
            m_Value = value.ToString();
            m_FloatValue = m_IntValue;
            m_BoolValue = value;
        }

        private void InnerInitStringValue(string value)
        {
            Parse(value);
        }

        public string Parse(string pattern)
        {
            m_Value = pattern;

            IntParser p1 = References.Require<IntParser>();
            FloatParser p2 = References.Require<FloatParser>();
            BoolParser p3 = References.Require<BoolParser>();

            p1.LogLv = LogLevel.Ignore;
            p2.LogLv = LogLevel.Ignore;
            p3.LogLv = LogLevel.Ignore;

            m_IntValue = p1.Parse(m_Value);
            m_FloatValue = p2.Parse(m_Value);
            m_BoolValue = p3.Parse(m_Value);

            References.Release(p1);
            References.Release(p2);
            References.Release(p3);

            return m_Value;
        }

        object IParser.Parse(string pattern)
        {
            return Parse(pattern);
        }

        public override string ToString()
        {
            return m_Value;
        }

        public override int GetHashCode()
        {
            return m_Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            IParser parser = obj as IParser;
            if (parser != null)
            {
                if (m_Value != null)
                    return m_Value.Equals(parser.Value);
                else
                    return parser.Value == null;
            }
            else
            {
                if (m_Value != null)
                    return m_Value.Equals(obj);
                else
                    return obj == null;
            }
        }

        public void Release()
        {
            References.Release(this);
        }

        protected override void OnReleaseFromPool()
        {
            base.OnReleaseFromPool();
            foreach (var item in m_Parsers)
                References.Release(item.Value);
            m_Parsers.Clear();
            m_Value = default;
            m_IntValue = default;
            m_FloatValue = default;
            m_BoolValue = default;
        }

        public static bool operator ==(UniversalParser src, object tar)
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

        public static bool operator !=(UniversalParser src, object tar)
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

        public static implicit operator string(UniversalParser parser)
        {
            return parser != null ? parser.m_Value : default;
        }

        public static implicit operator UniversalParser(string value)
        {
            UniversalParser parser = References.Require<UniversalParser>();
            parser.InnerInitStringValue(value);
            return parser;
        }

        public static implicit operator int(UniversalParser parser)
        {
            return parser != null ? parser.m_IntValue : default;
        }

        public static implicit operator UniversalParser(int value)
        {
            UniversalParser parser = References.Require<UniversalParser>();
            parser.InnerInitIntValue(value);
            return parser;
        }

        public static implicit operator float(UniversalParser parser)
        {
            return parser != null ? parser.m_FloatValue : default;
        }

        public static implicit operator UniversalParser(float value)
        {
            UniversalParser parser = References.Require<UniversalParser>();
            parser.InnerInitFloatValue(value);
            return parser;
        }

        public static implicit operator bool(UniversalParser parser)
        {
            return parser != null ? parser.m_BoolValue : default;
        }

        public static implicit operator UniversalParser(bool value)
        {
            UniversalParser parser = References.Require<UniversalParser>();
            parser.InnerInitBoolValue(value);
            return parser;
        }
    }
}

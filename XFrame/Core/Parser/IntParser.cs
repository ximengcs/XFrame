﻿using XFrame.Modules.Diagnotics;
using XFrame.Modules.Pools;

namespace XFrame.Core
{
    public class IntParser : IParser<int>
    {
        private int m_Value;
        public int Value => m_Value;
        public LogLevel LogLv { get; set; }

        object IParser.Value => m_Value;

        int IPoolObject.PoolKey => default;

        public int Parse(string pattern)
        {
            if (string.IsNullOrEmpty(pattern) || !int.TryParse(pattern, out m_Value))
            {
                m_Value = default;
                Log.Print(LogLv, "XFrame", $"IntParser parse failure. {pattern}");
            }

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

        void IPoolObject.OnCreate()
        {

        }

        void IPoolObject.OnRequest()
        {
            LogLv = LogLevel.Warning;
            m_Value = default;
        }

        void IPoolObject.OnRelease()
        {

        }

        void IPoolObject.OnDelete()
        {

        }

        public static bool operator ==(IntParser src, object tar)
        {
            return src.Equals(tar);
        }

        public static bool operator !=(IntParser src, object tar)
        {
            return !src.Equals(tar);
        }

        public static implicit operator int(IntParser parser)
        {
            return parser.m_Value;
        }

        public static implicit operator IntParser(int value)
        {
            IntParser parser = new IntParser();
            parser.m_Value = value;
            return parser;
        }
    }
}

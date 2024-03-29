﻿using XFrame.Modules.Diagnotics;
using XFrame.Modules.Pools;

namespace XFrame.Core
{
    public class FloatParser : IParser<float>
    {
        private float m_Value;
        public float Value => m_Value;
        public LogLevel LogLv { get; set; }

        object IParser.Value => m_Value;

        int IPoolObject.PoolKey => default;

        public float Parse(string pattern)
        {
            if (string.IsNullOrEmpty(pattern) || !float.TryParse(pattern, out m_Value))
            {
                m_Value = default;
                Log.Print(LogLv, "XFrame", $"FloatParser parse failure. {pattern}");
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
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            IParser parser = obj as IParser;
            if (parser != null)
            {
                return m_Value.Equals(parser.Value);
            }
            else
            {
                if (obj is int)
                    return m_Value.Equals((int)obj);
                else
                    return m_Value.Equals((float)obj);
            }
        }

        void IPoolObject.OnCreate()
        {

        }

        void IPoolObject.OnRequest()
        {
            m_Value = default;
            LogLv = LogLevel.Warning;
        }

        void IPoolObject.OnRelease()
        {

        }

        void IPoolObject.OnDelete()
        {

        }

        public static bool operator ==(FloatParser src, object tar)
        {
            return src.Equals(tar);
        }

        public static bool operator !=(FloatParser src, object tar)
        {
            return !src.Equals(tar);
        }

        public static implicit operator float(FloatParser parser)
        {
            return parser.m_Value;
        }

        public static implicit operator FloatParser(int value)
        {
            FloatParser parser = new FloatParser();
            parser.m_Value = value;
            return parser;
        }
    }
}

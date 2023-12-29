using System;
using XFrame.Modules.Pools;
using System.ComponentModel;
using XFrame.Modules.Diagnotics;

namespace XFrame.Core
{
    public class EnumParser<T> : PoolObjectBase, IParser<T> where T : Enum
    {
        private T m_Value;
        public T Value => m_Value;
        public LogLevel LogLv { get; set; }

        object IParser.Value => m_Value;

        public T Parse(string pattern)
        {
            if (!string.IsNullOrEmpty(pattern) && Enum.TryParse(typeof(T), pattern, out object value))
            {
                m_Value = (T)value;
            }
            else
            {
                InnerSetDefault();
                Log.Print(LogLv, "XFrame", $"EnumParser {typeof(T).Name} parse failure. {pattern}");
            }

            return m_Value;
        }

        private void InnerSetDefault()
        {
            DefaultValueAttribute attr = XModule.Type.GetAttribute<DefaultValueAttribute>(typeof(T));
            if (attr != null)
                m_Value = (T)attr.Value;
            else
                m_Value = default;
        }

        object IParser.Parse(string pattern)
        {
            return Parse(pattern);
        }

        public void Release()
        {
            References.Release(this);
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
            LogLv = LogLevel.Warning;
            InnerSetDefault();
        }

        public static bool operator ==(EnumParser<T> src, object tar)
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

        public static bool operator !=(EnumParser<T> src, object tar)
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

        public static implicit operator EnumParser<T>(T value)
        {
            EnumParser<T> parser = References.Require<EnumParser<T>>();
            parser.m_Value = value;
            return parser;
        }

        public static implicit operator T(EnumParser<T> value)
        {
            return value != null ? value.m_Value : default;
        }
    }
}

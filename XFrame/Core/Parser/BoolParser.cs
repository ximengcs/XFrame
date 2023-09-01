using XFrame.Modules.Pools;
using XFrame.Modules.Diagnotics;

namespace XFrame.Core
{
    public class BoolParser : IParser<bool>
    {
        private bool m_Value;
        public bool Value => m_Value;
        public LogLevel LogLv { get; set; }

        object IParser.Value => m_Value;

        int IPoolObject.PoolKey => default;

        public bool Parse(string pattern)
        {
            if (string.IsNullOrEmpty(pattern) || !TryParse(pattern, out m_Value))
            {
                m_Value = default;
                Log.Print(LogLv, "XFrame", $"BoolParser parse failure. {pattern}");
            }

            return m_Value;
        }

        public static bool TryParse(string pattern, out bool value)
        {
            if (!bool.TryParse(pattern, out value))
            {
                if (IntParser.TryParse(pattern, out int intResult))
                    return intResult != 0 ? true : false;
                else
                    return false;
            }
            else
            {
                return true;
            }
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

        }

        void IPoolObject.OnRelease()
        {
            LogLv = LogLevel.Warning;
            m_Value = default;
        }

        void IPoolObject.OnDelete()
        {

        }

        public static bool operator ==(BoolParser src, object tar)
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

        public static bool operator !=(BoolParser src, object tar)
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

        public static implicit operator bool(BoolParser parser)
        {
            return parser != null ? parser.m_Value : default;
        }

        public static implicit operator BoolParser(bool value)
        {
            BoolParser parser = new BoolParser();
            parser.m_Value = value;
            return parser;
        }
    }
}

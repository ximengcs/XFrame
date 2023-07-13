using XFrame.Modules.Diagnotics;
using XFrame.Modules.Pools;

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
            if (string.IsNullOrEmpty(pattern) || !bool.TryParse(pattern, out m_Value))
            {
                if (int.TryParse(pattern, out int intResult))
                {
                    m_Value = intResult != 0 ? true : false;
                }
                else
                {
                    m_Value = default;
                    Log.Print(LogLv, "XFrame", $"BoolParser parse failure. {pattern}");
                }
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
            return src.Equals(tar);
        }

        public static bool operator !=(BoolParser src, object tar)
        {
            return !src.Equals(tar);
        }

        public static implicit operator bool(BoolParser parser)
        {
            return parser.m_Value;
        }

        public static implicit operator BoolParser(bool value)
        {
            BoolParser parser = new BoolParser();
            parser.m_Value = value;
            return parser;
        }
    }
}

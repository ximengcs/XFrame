using XFrame.Modules.Pools;
using XFrame.Modules.Diagnotics;

namespace XFrame.Core
{
    public class IntOrHashParser : IntParser
    {
        private string m_Origin;

        public override int Parse(string pattern)
        {
            m_Origin = pattern;
            if (string.IsNullOrEmpty(pattern))
            {
                m_Value = default;
                Log.Print(LogLv, "XFrame", $"IntParser parse failure. {pattern}");
            }
            else
            {
                if (!TryParse(pattern, out m_Value))
                    m_Value = pattern.GetHashCode();
            }

            return m_Value;
        }

        public override bool Equals(object obj)
        {
            IParser parser = obj as IParser;
            object value = parser != null ? parser.Value : obj;
            if (value is int intValue)
                return m_Value.Equals(intValue);
            else if (value is string strValue)
                return m_Origin.Equals(strValue);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return m_Origin.GetHashCode();
        }

        public static bool operator ==(IntOrHashParser src, object tar)
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

        public static bool operator !=(IntOrHashParser src, object tar)
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

        public static implicit operator int(IntOrHashParser parser)
        {
            return parser != null ? parser.m_Value : default;
        }

        public static implicit operator IntOrHashParser(int value)
        {
            IntOrHashParser parser = References.Require<IntOrHashParser>();
            parser.m_Value = value;
            parser.m_Origin = value.ToString();
            return parser;
        }

        public static implicit operator string(IntOrHashParser parser)
        {
            return parser != null ? parser.m_Origin : default;
        }

        public static implicit operator IntOrHashParser(string value)
        {
            IntOrHashParser parser = References.Require<IntOrHashParser>();
            parser.Parse(value);
            return parser;
        }
    }
}

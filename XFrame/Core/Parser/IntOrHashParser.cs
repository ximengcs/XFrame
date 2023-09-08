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
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
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

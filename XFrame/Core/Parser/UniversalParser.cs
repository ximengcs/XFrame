
namespace XFrame.Core
{
    public class UniversalParser : IParser<string>
    {
        private string m_Value;
        private int m_IntValue;
        private float m_FloatValue;
        private bool m_BoolValue;

        public string Value => m_Value;
        public int IntValue => m_IntValue;
        public float FloatValue => m_FloatValue;
        public bool BoolValue => m_BoolValue;

        object IParser.Value => m_Value;

        public string Parse(string pattern)
        {
            m_Value = pattern;
            m_IntValue = ParserModule.Inst.INT.Parse(m_Value);
            m_FloatValue = ParserModule.Inst.FLOAT.Parse(m_Value);
            m_BoolValue = ParserModule.Inst.BOOL.Parse(m_Value);
            return m_Value;
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
            return parser.Value;
        }

        public static implicit operator UniversalParser(string value)
        {
            UniversalParser parser = new UniversalParser();
            parser.Parse(value);
            return parser;
        }
    }
}

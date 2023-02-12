
namespace XFrame.Core
{
    public class IntParser : IParser
    {
        private int m_Value;

        public int Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public void Init(string pattern)
        {
            m_Value = Parse(pattern);
        }

        public static int Parse(string pattern)
        {
            if (int.TryParse(pattern, out int result))
                return result;
            else
                return default;
        }

        public static implicit operator int(IntParser parser)
        {
            return parser.Value;
        }
    }
}

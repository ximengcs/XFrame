
namespace XFrame.Core
{
    public class FloatParser : IParser
    {
        private float m_Value;

        public float Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public void Init(string pattern)
        {
            m_Value = float.Parse(pattern);
        }

        public static implicit operator float(FloatParser parser)
        {
            return parser.Value;
        }
    }
}

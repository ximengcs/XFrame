using XFrame.Modules.Diagnotics;

namespace XFrame.Core
{
    public class IntParser : IParser<int>
    {
        public int Value { get; private set; }

        object IParser.Value => Value;

        public int Parse(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                return default;
            if (int.TryParse(pattern, out int result))
                Value = result;

            return Value;
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
            return parser.Value;
        }

        public static implicit operator IntParser(int value)
        {
            IntParser parser = new IntParser();
            parser.Value = value;
            return parser;
        }
    }
}

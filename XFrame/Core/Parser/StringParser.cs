
namespace XFrame.Core
{
    public class StringParser : IParser<string>
    {
        public string Value { get; private set; }

        public string Parse(string pattern)
        {
            Value = pattern;
            return Value;
        }
    }
}

using XFrame.Modules.Diagnotics;

namespace XFrame.Core
{
    public class IntParser : IParser<int>
    {
        public int Value { get; private set; }

        public int Parse(string pattern)
        {
            if (int.TryParse(pattern, out int result))
                Value = result;
            else
                Log.Error("Parse", "parse error");
            return Value;
        }
    }
}

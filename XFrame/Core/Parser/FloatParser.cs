using XFrame.Modules.Diagnotics;

namespace XFrame.Core
{
    public class FloatParser : IParser<float>
    {
        public float Value { get; private set; }

        public float Parse(string pattern)
        {
            if (float.TryParse(pattern, out float result))
                Value = result;
            else
                Log.Error("Parse", "parse error");
            return Value;
        }
    }
}

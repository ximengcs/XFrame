using System;
using XFrame.Modules.Diagnotics;

namespace XFrame.Core
{
    public class EnumParser<T> : IParser<T> where T : Enum
    {
        public T Value { get; private set; }

        public T Parse(string pattern)
        {
            if (Enum.TryParse(typeof(T), pattern, out object result))
            {
                Value = (T)result;
            }
            else
            {
                Log.Error("Parse", "parse error");
            }
            return Value;
        }
    }
}

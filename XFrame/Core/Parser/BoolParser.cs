﻿
using XFrame.Modules.Diagnotics;

namespace XFrame.Core
{
    public class BoolParser : IParser<bool>
    {
        public bool Value { get; private set; }

        object IParser.Value => Value;

        public bool Parse(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                return default;
            if (bool.TryParse(pattern, out bool result))
                Value = result;
            else if (int.TryParse(pattern, out int intResult))
                Value = intResult != 0 ? true : false;
            else
                Log.Error("Parse", $"parse error {pattern}");
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

        public static bool operator ==(BoolParser src, object tar)
        {
            return src.Equals(tar);
        }

        public static bool operator !=(BoolParser src, object tar)
        {
            return !src.Equals(tar);
        }

        public static implicit operator bool(BoolParser parser)
        {
            return parser.Value;
        }

        public static implicit operator BoolParser(bool value)
        {
            BoolParser parser = new BoolParser();
            parser.Value = value;
            return parser;
        }
    }
}

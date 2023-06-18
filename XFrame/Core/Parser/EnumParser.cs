using System;
using System.ComponentModel;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.XType;

namespace XFrame.Core
{
    public class EnumParser<T> : IParser<T> where T : Enum
    {
        public T Value { get; private set; }

        object IParser.Value => Value;

        public T Parse(string pattern)
        {
            if (!string.IsNullOrEmpty(pattern))
            {
                if (Enum.TryParse(typeof(T), pattern, out object result))
                {
                    Value = (T)result;
                    return Value;
                }
            }

            DefaultValueAttribute attr = TypeModule.Inst.GetAttribute<DefaultValueAttribute>(typeof(T));
            if (attr != null)
                Value = (T)attr.Value;
            else
                Value = default;
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

        public static bool operator ==(EnumParser<T> src, object tar)
        {
            return src.Equals(tar);
        }

        public static bool operator !=(EnumParser<T> src, object tar)
        {
            return !src.Equals(tar);
        }

        public static implicit operator EnumParser<T>(T value)
        {
            EnumParser<T> parser = new EnumParser<T>();
            parser.Value = value;
            return parser;
        }

        public static implicit operator T(EnumParser<T> value)
        {
            return value.Value;
        }
    }
}

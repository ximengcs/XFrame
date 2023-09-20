
using XFrame.Modules.Pools;

namespace XFrame.Core
{
    public class StringParser : IParser<string>
    {
        public string Value { get; private set; }

        object IParser.Value => Value;

        int IPoolObject.PoolKey => default;

        IPool IPoolObject.InPool { get; set; }

        public string Parse(string pattern)
        {
            Value = string.IsNullOrEmpty(pattern) ? string.Empty : pattern;
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

        public void Release()
        {
            References.Release(this);
        }

        void IPoolObject.OnCreate()
        {

        }

        void IPoolObject.OnRequest()
        {
            Value = null;
        }

        void IPoolObject.OnRelease()
        {

        }

        void IPoolObject.OnDelete()
        {

        }

        public static bool operator ==(StringParser src, object tar)
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

        public static bool operator !=(StringParser src, object tar)
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

        public static implicit operator string(StringParser parser)
        {
            return parser != null ? parser.Value : default;
        }

        public static implicit operator StringParser(string value)
        {
            StringParser parser = References.Require<StringParser>();
            parser.Value = string.IsNullOrEmpty(value) ? string.Empty : value;
            return parser;
        }
    }
}

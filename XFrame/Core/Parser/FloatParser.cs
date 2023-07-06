﻿using XFrame.Modules.Pools;

namespace XFrame.Core
{
    public class FloatParser : IParser<float>
    {
        public float Value { get; private set; }

        object IParser.Value => Value;

        int IPoolObject.PoolKey => default;

        public float Parse(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                return default;
            if (float.TryParse(pattern, out float result))
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
            if (parser != null)
            {
                return Value.Equals(parser.Value);
            }
            else
            {
                if (obj is int)
                    return Value.Equals((int)obj);
                else
                    return Value.Equals((float)obj);
            }
        }

        void IPoolObject.OnCreate()
        {

        }

        void IPoolObject.OnRequest()
        {
            Value = 0;
        }

        void IPoolObject.OnRelease()
        {

        }

        void IPoolObject.OnDelete()
        {

        }

        public static bool operator ==(FloatParser src, object tar)
        {
            return src.Equals(tar);
        }

        public static bool operator !=(FloatParser src, object tar)
        {
            return !src.Equals(tar);
        }

        public static implicit operator float(FloatParser parser)
        {
            return parser.Value;
        }

        public static implicit operator FloatParser(int value)
        {
            FloatParser parser = new FloatParser();
            parser.Value = value;
            return parser;
        }
    }
}

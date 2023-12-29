using System.Globalization;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Pools;

namespace XFrame.Core
{
    public class FloatParser : PoolObjectBase, IParser<float>
    {
        private float m_Value;
        public float Value => m_Value;
        public LogLevel LogLv { get; set; }

        object IParser.Value => m_Value;

        public float Parse(string pattern)
        {
            if (string.IsNullOrEmpty(pattern) || !TryParse(pattern, out m_Value))
            {
                m_Value = default;
                Log.Print(LogLv, "XFrame", $"FloatParser parse failure. {pattern}");
            }

            return m_Value;
        }

        public static bool TryParse(string pattern, out float value)
        {
            return float.TryParse(pattern, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out value);
        }

        object IParser.Parse(string pattern)
        {
            return Parse(pattern);
        }

        public override string ToString()
        {
            return m_Value.ToString();
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
                return m_Value.Equals(parser.Value);
            }
            else
            {
                if (obj is int)
                    return m_Value.Equals((int)obj);
                else
                    return m_Value.Equals((float)obj);
            }
        }

        public void Release()
        {
            References.Release(this);
        }

        protected override void OnRequestFromPool()
        {
            base.OnRequestFromPool();
            PoolKey = 0;
            m_Value = default;
            LogLv = LogLevel.Warning;
        }

        public static bool operator ==(FloatParser src, object tar)
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

        public static bool operator !=(FloatParser src, object tar)
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

        public static implicit operator float(FloatParser parser)
        {
            return parser != null ? parser.m_Value : default;
        }

        public static implicit operator FloatParser(int value)
        {
            FloatParser parser = References.Require<FloatParser>();
            parser.m_Value = value;
            return parser;
        }
    }
}

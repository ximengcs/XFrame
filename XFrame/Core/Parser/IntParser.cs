using System.Globalization;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Pools;

namespace XFrame.Core
{
    public class IntParser : PoolObjectBase, IParser<int>
    {
        protected int m_Value;
        public int Value => m_Value;
        public LogLevel LogLv { get; set; }

        object IParser.Value => m_Value;

        public virtual int Parse(string pattern)
        {
            if (string.IsNullOrEmpty(pattern) || !TryParse(pattern, out m_Value))
            {
                m_Value = default;
                Log.Print(LogLv, "XFrame", $"IntParser parse failure. {pattern}");
            }

            return m_Value;
        }

        public static bool TryParse(string pattern, out int value)
        {
            return int.TryParse(pattern, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
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
            return m_Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            IParser parser = obj as IParser;
            return parser != null ? m_Value.Equals(parser.Value) : m_Value.Equals(obj);
        }

        public void Release()
        {
            References.Release(this);
        }

        protected internal override void OnRequestFromPool()
        {
            base.OnRequestFromPool();
            PoolKey = 0;
            LogLv = LogLevel.Warning;
            m_Value = default;
        }

        public static bool operator ==(IntParser src, object tar)
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

        public static bool operator !=(IntParser src, object tar)
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

        public static implicit operator int(IntParser parser)
        {
            return parser != null ? parser.m_Value : default;
        }

        public static implicit operator IntParser(int value)
        {
            IntParser parser = References.Require<IntParser>();
            parser.m_Value = value;
            return parser;
        }
    }
}

using XFrame.Modules.Pools;
using System.Collections.Generic;
using XFrame.Modules.Diagnotics;

namespace XFrame.Core
{
    public partial class AreaParser : PoolObjectBase, IParser<IEnumerable<string>>
    {
        public const char SPLIT = '@';
        public const char SPLIT2 = '!';
        public const char SPLIT3 = '-';

        public const string KEY1 = "add";
        public const string KEY2 = "remove";

        private string m_RawValue;
        private bool m_ValueDirty;
        private List<string> m_Values;
        private HashSet<string> m_MapValues;
        private List<Info> m_AddInfos;
        private List<Info> m_RemoveInfos;

        public char Split { get; set; }
        public char Split2 { get; set; }
        public char Split3 { get; set; }

        object IParser.Value => Value;

        public IEnumerable<string> Value
        {
            get
            {
                InnerCalculateValue();
                return m_Values;
            }
        }

        public static bool CheckValidHead(string content)
        {
            return content.StartsWith(KEY1) || content.StartsWith(KEY2);
        }

        protected internal override void OnCreateFromPool()
        {
            base.OnCreateFromPool();
            m_MapValues = new HashSet<string>();
            m_Values = new List<string>();
            m_AddInfos = new List<Info>();
            m_RemoveInfos = new List<Info>();
        }

        protected internal override void OnRequestFromPool()
        {
            base.OnRequestFromPool();
            Split = SPLIT;
            Split2 = SPLIT2;
            Split3 = SPLIT3;
        }

        protected internal override void OnReleaseFromPool()
        {
            base.OnReleaseFromPool();
            m_Values.Clear();
            m_MapValues.Clear();
            m_AddInfos.Clear();
            m_RemoveInfos.Clear();
        }

        public IEnumerable<string> Parse(string pattern)
        {
            m_RawValue = pattern;
            m_ValueDirty = true;
            if (!string.IsNullOrEmpty(pattern))
            {
                string[] items = pattern.Split(Split);
                foreach (string item in items)
                {
                    string[] kv = item.Split(Split2);
                    if (kv.Length == 2)
                    {
                        Info info = new Info(kv[1], Split3);
                        string k = kv[0];
                        if (k == KEY1)
                        {
                            m_AddInfos.Add(info);
                        }
                        else if (k == KEY2)
                        {
                            m_RemoveInfos.Add(info);
                        }
                        else
                        {
                            Log.Error("XFrame", $"NumAreaParse Error {pattern}");
                        }
                    }
                    else
                    {
                        Info info = new Info(kv[0], Split3);
                        m_AddInfos.Add(info);
                    }
                }
            }
            return Value;
        }

        object IParser.Parse(string pattern)
        {
            return Parse(pattern);
        }

        private void InnerCalculateValue()
        {
            if (m_ValueDirty)
            {
                m_MapValues.Clear();
                m_Values.Clear();
                HashSet<string> addTmp = new HashSet<string>();
                HashSet<string> removeTmp = new HashSet<string>();

                foreach (Info info in m_AddInfos)
                    info.Calcalute(addTmp);
                foreach (Info info in m_RemoveInfos)
                    info.Calcalute(removeTmp);

                foreach (string str in addTmp)
                {
                    if (!removeTmp.Contains(str))
                    {
                        m_Values.Add(str);
                        m_MapValues.Add(str);
                    }
                }

                m_ValueDirty = false;
            }
        }

        public bool Has(string str)
        {
            return m_MapValues.Contains(str);
        }

        public override string ToString()
        {
            return m_RawValue;
        }

        public override int GetHashCode()
        {
            return m_RawValue.GetHashCode();
        }

        public void Release()
        {
            References.Release(this);
        }

        public override bool Equals(object obj)
        {
            AreaParser parser = obj as AreaParser;
            if (parser != null)
            {
                return m_RawValue.Equals(parser.m_RawValue);
            }
            else
            {
                string strValue = obj as string;
                return strValue == m_RawValue;
            }
        }

        public static bool operator ==(AreaParser src, object tar)
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

        public static bool operator !=(AreaParser src, object tar)
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

        public static implicit operator string(AreaParser parser)
        {
            return parser != null ? parser.m_RawValue : default;
        }

        public static implicit operator AreaParser(string value)
        {
            AreaParser parser = References.Require<AreaParser>();
            parser.Parse(value);
            return parser;
        }
    }
}

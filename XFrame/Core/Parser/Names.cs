using System;
using XFrame.Core;
using XFrame.Modules.Pools;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using System.Collections;

namespace UnityXFrame.Core.Parser
{
    public class Names : MapParser<IntOrHashParser, AreaParser>, IEnumerable<string>
    {
        private Comparison<IntOrHashParser> m_Compare;
        private List<IntOrHashParser> m_Keys;
        private bool m_ValueDirty;
        private List<string> m_Values;
        private HashSet<string> m_ValueMap;

        private List<string> Values
        {
            get
            {
                InnerCalculateValue();
                return m_Values;
            }
        }

        private void InnerCalculateValue()
        {
            if (m_ValueDirty)
            {
                m_Values.Clear();
                if (m_Compare != null)
                    m_Keys.Sort(m_Compare);
                InnerCollectNumArea(string.Empty, 0);
                m_ValueDirty = false;
            }
        }

        private bool InnerCollectNumArea(string prev, int currentIndex)
        {
            if (currentIndex >= m_Keys.Count)
                return false;
            IntOrHashParser key = m_Keys[currentIndex];
            AreaParser numArea = Get(key);
            bool isRoot = string.IsNullOrEmpty(prev);
            foreach (string str in numArea.Value)
            {
                string thisValue;
                if (key != Name.AVATAR)
                    thisValue = isRoot ? $"{key}{Split2}{str}" : $"{prev}{Split}{key}{Split2}{str}";
                else
                    thisValue = isRoot ? str : $"{prev}{Split}{str}";
                bool next = InnerCollectNumArea(thisValue, currentIndex + 1);
                if (!isRoot && !next)
                {
                    m_Values.Add(thisValue);
                    m_ValueMap.Add(thisValue);
                }
            }
            return true;
        }

        protected internal override void OnCreateFromPool()
        {
            base.OnCreateFromPool();
            Split = Name.SPLIT;
            Split2 = Name.SPLIT2;
            m_Keys = new List<IntOrHashParser>();
            m_Values = new List<string>();
            m_ValueMap = new HashSet<string>();
        }

        protected internal override void OnReleaseFromPool()
        {
            base.OnReleaseFromPool();
            m_Keys.Clear();
        }

        protected override void InnerParseItem(out IntOrHashParser kParser, out AreaParser vParser, string[] pItem)
        {
            m_ValueDirty = true;
            if (pItem.Length == 1)
            {
                IntOrHashParser key = Name.AVATAR;
                if (Has(key))
                    Log.Error($"Name format error, multi type 0");
                kParser = key;
                vParser = References.Require<AreaParser>();
                vParser.Parse(pItem[0]);
            }
            else
            {
                kParser = References.Require<IntOrHashParser>();
                vParser = References.Require<AreaParser>();
                kParser.Parse(pItem[0]);
                vParser.Parse(pItem[1]);
            }

            m_Keys.Add(kParser);
        }

        public void SetSort(Comparison<IntOrHashParser> compareFunc)
        {
            m_Compare = compareFunc;
        }

        public bool Has(string name)
        {
            return m_ValueMap.Contains(name);
        }

        public bool Has(Name name)
        {
            return m_ValueMap.Contains(name);
        }

        public override bool Equals(object obj)
        {
            Names parser = obj as Names;
            if (parser == null)
            {
                string strValue = obj as string;
                if (strValue == m_Origin)
                    return true;

                if (!string.IsNullOrEmpty(strValue))
                    parser = Names.Create(strValue);

                if (parser != null)
                {
                    bool equals = InnerCompareName(this, parser);
                    parser.Release();
                    return equals;
                }
            }
            else
            {
                return InnerCompareName(this, parser);
            }

            return false;
        }

        private static bool InnerCompareName(Names name1, Names name2)
        {
            if (name1.Count != name2.Count)
                return false;
            foreach (var entry in name2.Value)
            {
                if (name1.TryGet(entry.Key, out AreaParser value))
                {
                    if (entry.Value != value)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            int result = 0;
            foreach (var entry in Value)
                result += entry.GetHashCode();
            return result;
        }

        public static Names Create(string pattern)
        {
            Names name = References.Require<Names>();
            name.Split = Name.SPLIT;
            name.Split2 = Name.SPLIT2;
            name.Parse(pattern);
            return name;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        public static bool operator ==(Names src, object tar)
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

        public static bool operator !=(Names src, object tar)
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

        public static implicit operator string(Names parser)
        {
            return parser != null ? parser.m_Origin : default;
        }

        public static implicit operator Names(string value)
        {
            return Create(value);
        }
    }
}

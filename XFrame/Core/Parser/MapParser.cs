using System;
using XFrame.Modules.Pools;
using System.Collections.Generic;

namespace XFrame.Core
{
    public class MapParser<K, V> : PoolObjectBase, IParser<Dictionary<K, V>> where K : IParser where V : IParser
    {
        private const char SPLIT = ',';
        private const char SPLIT2 = '|';
        private Dictionary<K, V> m_Value;

        public Dictionary<K, V> Value => m_Value;

        object IParser.Value => m_Value;
        IPool IPoolObject.InPool { get; set; }

        protected string m_Origin;
        private char m_Split;
        private char m_Split2;

        public char Split
        {
            get => m_Split;
            set
            {
                if (m_Split != value)
                {
                    m_Split = value;
                    Parse(m_Origin);
                }
            }
        }

        public char Split2
        {
            get => m_Split2;
            set
            {
                if (m_Split2 != value)
                {
                    m_Split2 = value;
                    Parse(m_Origin);
                }
            }
        }

        public V this[K key] => m_Value[key];

        public int Count => m_Value.Count;

        protected MapParser()
        {
            m_Split = SPLIT;
            m_Split2 = SPLIT2;
            m_Value = new Dictionary<K, V>();
        }

        public static MapParser<K, V> Create(char splitchar, char splitchar2)
        {
            MapParser<K, V> parser = References.Require<MapParser<K, V>>();
            parser.m_Split = splitchar;
            parser.m_Split2 = splitchar2;
            return parser;
        }

        public V Get(K key)
        {
            if (m_Value.TryGetValue(key, out V value))
                return value;
            return default;
        }

        public bool Has(K key)
        {
            return m_Value.ContainsKey(key);
        }

        public bool TryGet(K key, out V value)
        {
            return m_Value.TryGetValue(key, out value);
        }

        public Dictionary<K, V> Parse(string pattern)
        {
            m_Origin = pattern;

            if (!string.IsNullOrEmpty(pattern))
            {
                pattern = pattern.Trim('{', '}');
                string[] pMap = pattern.Split(m_Split);
                Type kType = typeof(K);
                Type vType = typeof(V);
                for (int i = 0; i < pMap.Length; i++)
                {
                    string pItemStr = pMap[i];
                    K kParser;
                    V vParser;
                    if (!string.IsNullOrEmpty(pItemStr))
                    {
                        string[] pItem = pItemStr.Split(m_Split2);
                         InnerParseItem(out kParser, out vParser, pItem);
                    }
                    else
                    {
                        kParser = (K)References.Require(kType);
                        vParser = (V)References.Require(vType);
                    }
                    if (m_Value.ContainsKey(kParser))
                        References.Release(kParser);
                    m_Value[kParser] = vParser;
                }
            }

            return m_Value;
        }

        public void Release()
        {
            References.Release(this);
        }

        protected virtual void InnerParseItem(out K kParser, out V vParser, string[] pItem)
        {
            kParser = References.Require<K>();
            vParser = References.Require<V>();
            kParser.Parse(pItem[0]);
            if (pItem.Length > 1)
                vParser.Parse(pItem[1]);
        }

        object IParser.Parse(string pattern)
        {
            return Parse(pattern);
        }

        protected internal override void OnReleaseFromPool()
        {
            base.OnReleaseFromPool();
            foreach (IParser parser in m_Value.Values)
                References.Release(parser);
            m_Value.Clear();
        }

        public override string ToString()
        {
            return m_Origin;
        }
    }
}

using System;
using XFrame.Modules.Pools;
using System.Collections.Generic;
using System.Data;

namespace XFrame.Core
{
    public class MapParser<K, V> : IParser<Dictionary<K, V>> where K : IParser where V : IParser
    {
        private const char SPLIT = ',';
        private const char SPLIT2 = '|';
        private Dictionary<K, V> m_Value;

        public Dictionary<K, V> Value => m_Value;

        object IParser.Value => m_Value;

        public int PoolKey => default;

        private string m_Origin;
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

        public int Count => m_Value.Count;

        public MapParser()
        {
            m_Split = SPLIT;
            m_Split2 = SPLIT2;
        }

        public MapParser(char splitchar, char splitchar2)
        {
            m_Split = splitchar;
            m_Split2 = splitchar2;
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
            if (m_Value == null)
                m_Value = new Dictionary<K, V>();

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
                        kParser.Parse(null);
                        vParser.Parse(null);
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
            else
                vParser.Parse(null);
        }

        object IParser.Parse(string pattern)
        {
            return Parse(pattern);
        }

        void IPoolObject.OnCreate()
        {

        }

        void IPoolObject.OnDelete()
        {

        }

        void IPoolObject.OnRelease()
        {
            foreach (IParser parser in m_Value.Values)
                References.Release(parser);
            m_Value.Clear();
        }

        void IPoolObject.OnRequest()
        {

        }

        public override string ToString()
        {
            return m_Origin;
        }
    }
}

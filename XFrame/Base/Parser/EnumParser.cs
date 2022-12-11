using System;
using XFrame.Modules;
using System.Collections.Generic;

namespace XFrame.Core
{
    public partial class EnumParser<T, VT> : IParser where T : Enum where VT : IParser
    {
        private const char ITEM_SPLIT = ';';
        private const char INNER_ITEM_SPLIT = '|';

        private Type m_Type;
        private string m_PatternRaw;
        private Dictionary<int, List<VT>> m_Pattern;

        public static EnumParser<T, VT> Create(string pattern)
        {
            EnumParser<T, VT> parser = new EnumParser<T, VT>();
            parser.Init(pattern);
            return parser;
        }

        public void Init(string pattern)
        {
            m_Type = typeof(T);
            m_PatternRaw = pattern;
            m_Pattern = new Dictionary<int, List<VT>>();
            InnerAnalytic();
        }

        public bool Empty { get { return m_Pattern.Count == 0; } }

        public bool Has(T type)
        {
            return m_Pattern.ContainsKey(type.GetHashCode());
        }

        public VT Get(T type)
        {
            if (m_Pattern.TryGetValue(type.GetHashCode(), out List<VT> items))
                return items[0];
            return default;
        }

        private void InnerAnalytic()
        {
            if (string.IsNullOrEmpty(m_PatternRaw))
                return;

            string[] str = m_PatternRaw.Split(ITEM_SPLIT);
            foreach (string itemStr in str)
            {
                string[] itemStrs = itemStr.Split(INNER_ITEM_SPLIT);
                VT item = Activator.CreateInstance<VT>();
                T type = default;
                try
                {
                    string pa = itemStrs[0];
                    string head = pa.Substring(0, 1);
                    type = (T)Enum.Parse(m_Type, pa.Replace(head, head.ToUpper()));
                }
                catch (Exception e)
                {
                    Log.Error("XFrame", $"{m_Type.Name} {itemStrs[0]} ({itemStr}) Not Exist");
                    Log.Error("XFrame", e.ToString());
                }

                if (itemStrs.Length == 1)
                    item.Init(default);
                else
                    item.Init(itemStrs[1]);

                List<VT> list;
                if (!m_Pattern.TryGetValue(type.GetHashCode(), out list))
                {
                    list = new List<VT>();
                    m_Pattern[type.GetHashCode()] = list;
                }
                list.Add(item);
            }
        }
    }
}

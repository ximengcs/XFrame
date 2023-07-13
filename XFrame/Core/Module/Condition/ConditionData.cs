using XFrame.Core;
using System.Collections.Generic;

namespace XFrame.Modules.Conditions
{
    public struct ConditionData
    {
        private PairParser<IntParser, UniversalParser> m_First;

        public ArrayParser<PairParser<IntParser, UniversalParser>> Parser;

        public ConditionData(ArrayParser<PairParser<IntParser, UniversalParser>> parser)
        {
            m_First = null;
            Parser = parser;
        }

        public PairParser<IntParser, UniversalParser> First
        {
            get
            {
                if (m_First == null)
                {
                    foreach (var itemNode in Parser.Value)
                    {
                        m_First = itemNode.Value;
                        break;
                    }
                }
                return m_First;
            }
        }

        public bool Has(int target)
        {
            return Find(target) != null;
        }

        public PairParser<IntParser, UniversalParser> Find(int target)
        {
            foreach (var itemNode in Parser.Value)
            {
                if (itemNode.Value.Value.Key == target)
                {
                    return itemNode.Value.Value;
                }
            }
            return default;
        }

        public List<PairParser<IntParser, UniversalParser>> FindAll(int target)
        {
            var result = new List<PairParser<IntParser, UniversalParser>>(Parser.Value.Count);
            foreach (var itemNode in Parser.Value)
            {
                if (itemNode.Value.Value.Key == target)
                {
                    result.Add(itemNode.Value);
                }
            }
            return result;
        }

        public override string ToString()
        {
            return Parser.ToString();
        }
    }
}

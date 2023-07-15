using XFrame.Core;
using XFrame.Collections;
using System.Collections.Generic;

namespace XFrame.Modules.Conditions
{
    public partial struct ConditionData : IXEnumerable<PairParser<IntParser, UniversalParser>>
    {
        private PairParser<IntParser, UniversalParser> m_First;
        private PairParser<IntParser, UniversalParser> m_Last;
        private ArrayParser<PairParser<IntParser, UniversalParser>> m_Parser;

        public ArrayParser<PairParser<IntParser, UniversalParser>> Parser => m_Parser;
        public PairParser<IntParser, UniversalParser> First => m_First;
        public PairParser<IntParser, UniversalParser> Last => m_Last;

        public ConditionData(string originData)
        {
            m_Parser = new ArrayParser<PairParser<IntParser, UniversalParser>>();
            m_Parser.Parse(originData);

            if (!m_Parser.Empty)
            {
                m_First = m_Parser.Value.First.Value;
                m_Last = m_Parser.Value.Last.Value;
            }
            else
            {
                m_First = null;
                m_Last = null;
            }
        }

        public ConditionData(ArrayParser<PairParser<IntParser, UniversalParser>> parser)
        {
            m_Parser = parser;
            if (!m_Parser.Empty)
            {
                m_First = m_Parser.Value.First.Value;
                m_Last = m_Parser.Value.Last.Value;
            }
            else
            {
                m_First = null;
                m_Last = null;
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

        public IEnumerator<PairParser<IntParser, UniversalParser>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        public void SetIt(XItType type)
        {
            Parser.Value.SetIt(type);
        }
    }
}

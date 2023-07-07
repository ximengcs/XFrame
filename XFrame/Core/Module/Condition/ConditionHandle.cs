using System;
using System.Text.RegularExpressions;
using XFrame.Core;

namespace XFrame.Modules.Conditions
{
    public class ConditionHandle
    {
        private int m_Target;
        private UniversalParser m_Param;
        private IDataProvider m_Data;
        private ConditionGroupHandle m_Group;

        public int Target => m_Target;
        public UniversalParser Param => m_Param;
        public ConditionGroupHandle Group => m_Group;
        public IDataProvider Data => m_Data;
        public Action<object, object> UpdateEvent;

        internal ConditionHandle(ConditionGroupHandle group, PairParser<IntParser, UniversalParser> parser)
        {
            m_Group = group;
            Pair<IntParser, UniversalParser> pair = parser;
            m_Target = pair.Key;
            m_Param = pair.Value;
            m_Data = new DataProvider();
        }
    }
}

﻿using System;
using XFrame.Core;

namespace XFrame.Modules.Conditions
{
    public class ConditionHandle
    {
        private int m_Target;
        private UniversalParser m_Param;
        private IDataProvider m_Data;

        public int Target => m_Target;
        public UniversalParser Param => m_Param;
        public IDataProvider Data => m_Data;
        public Action<object, object> UpdateEvent;

        internal ConditionHandle(PairParser<IntParser, UniversalParser> parser)
        {
            Pair<IntParser, UniversalParser> pair = parser;
            m_Target = pair.Key;
            m_Param = pair.Value;
            m_Data = new DataProvider();
        }
    }
}

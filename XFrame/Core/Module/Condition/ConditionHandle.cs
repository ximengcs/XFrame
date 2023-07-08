using System;
using XFrame.Core;

namespace XFrame.Modules.Conditions
{
    public class ConditionHandle
    {
        private int m_Target;
        private UniversalParser m_Param;
        private IDataProvider m_Data;
        private ConditionGroupHandle m_Group;
        private Action<ConditionHandle> m_OnComplete;
        private bool m_Complete;

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
            m_Complete = false;
        }

        internal void MarkComplete()
        {
            m_Complete = true;
            m_OnComplete?.Invoke(this);
            m_OnComplete = null;
        }

        public void OnComplete(Action<ConditionHandle> callback)
        {
            if (m_Complete)
            {
                callback?.Invoke(this);
            }
            else
            {
                m_OnComplete += callback;
            }
        }
    }
}


using XFrame.Core;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Conditions
{
    /// <summary>
    /// 条件转换器
    /// </summary>
    public class ConditionParser : PoolObjectBase, IParser<ConditionData>
    {
        private ConditionData m_Value;

        public ConditionData Value => m_Value;

        object IParser.Value => m_Value;

        public ConditionData Parse(string pattern)
        {
            m_Value = new ConditionData(pattern);
            return m_Value;
        }

        protected internal override void OnReleaseFromPool()
        {
            base.OnReleaseFromPool();
            m_Value = default;
        }

        protected internal override void OnRequestFromPool()
        {
            base.OnRequestFromPool();
            PoolKey = 0;
        }

        object IParser.Parse(string pattern)
        {
            return Parse(pattern);
        }
    }
}

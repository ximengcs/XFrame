
using XFrame.Core;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Conditions
{
    /// <summary>
    /// 条件转换器
    /// </summary>
    public class ConditionParser : IParser<ConditionData>
    {
        private ConditionData m_Value;

        public ConditionData Value => m_Value;

        public int PoolKey => default;

        IPool IPoolObject.InPool { get; set; }
        public string MarkName { get; set; }

        object IParser.Value => m_Value;

        public ConditionData Parse(string pattern)
        {
            m_Value = new ConditionData(pattern);
            return m_Value;
        }

        void IPoolObject.OnCreate()
        {

        }

        void IPoolObject.OnDelete()
        {

        }

        void IPoolObject.OnRelease()
        {
            m_Value = default;
        }

        void IPoolObject.OnRequest()
        {

        }

        object IParser.Parse(string pattern)
        {
            return Parse(pattern);
        }
    }
}

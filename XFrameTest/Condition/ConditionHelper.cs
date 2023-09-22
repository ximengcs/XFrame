
using XFrame.Core;
using XFrame.Modules.Archives;
using XFrame.Modules.Conditions;
using XFrame.Modules.Pools;

namespace XFrameTest.Condition
{
    public class ConditionHelper : PoolObjectBase, IConditionHelper
    {
        private JsonArchive m_Archive;

        public ConditionHelper()
        {
            m_Archive = ModuleUtility.Archive.GetOrNew<JsonArchive>("condition_persist");
        }

        public int Type => 0;

        public bool CheckFinish(IConditionGroupHandle group)
        {
            return m_Archive.GetBool(group.Name);
        }

        public void MarkFinish(IConditionGroupHandle group)
        {
            m_Archive.SetBool(group.Name, true);
        }
    }
}

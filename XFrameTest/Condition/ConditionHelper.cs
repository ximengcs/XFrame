
using XFrame.Modules.Archives;
using XFrame.Modules.Conditions;

namespace XFrameTest.Condition
{
    public class ConditionHelper : IConditionHelper
    {
        private JsonArchive m_Archive;

        public ConditionHelper()
        {
            m_Archive = ArchiveModule.Inst.GetOrNew<JsonArchive>("condition_persist");
        }

        public bool CheckFinish(string groupName)
        {
            return m_Archive.GetBool(groupName);
        }

        public void MarkFinish(string groupName)
        {
            m_Archive.SetBool(groupName, true);
        }
    }
}

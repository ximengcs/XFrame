
using XFrame.Modules.Archives;
using XFrame.Modules.Conditions;
using XFrame.Modules.Pools;

namespace XFrameTest.Condition
{
    public class ConditionHelper : IConditionHelper
    {
        private JsonArchive m_Archive;

        public ConditionHelper()
        {
            m_Archive = ArchiveModule.Inst.GetOrNew<JsonArchive>("condition_persist");
        }

        public int Type => 0;

        public int PoolKey => throw new NotImplementedException();

        int IConditionHelper.Type => throw new NotImplementedException();

        int IPoolObject.PoolKey => throw new NotImplementedException();

        public bool CheckFinish(string groupName)
        {
            return m_Archive.GetBool(groupName);
        }

        public bool CheckFinish(IConditionGroupHandle group)
        {
            throw new NotImplementedException();
        }

        public void MarkFinish(string groupName)
        {
            m_Archive.SetBool(groupName, true);
        }

        public void MarkFinish(IConditionGroupHandle group)
        {
            throw new NotImplementedException();
        }

        void IPoolObject.OnCreate()
        {
            throw new NotImplementedException();
        }

        void IPoolObject.OnDelete()
        {
            throw new NotImplementedException();
        }

        void IPoolObject.OnRelease()
        {
            throw new NotImplementedException();
        }

        void IPoolObject.OnRequest()
        {
            throw new NotImplementedException();
        }
    }
}

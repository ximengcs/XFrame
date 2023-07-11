using XFrame.Core;
using XFrame.Modules.Conditions;
using XFrame.Modules.Diagnotics;

namespace XFrameTest.Condition
{
    public class Con1Compare : IConditionCompare
    {
        int IConditionCompare.Target => CondConst.COIN;

        public bool Check(IConditionHandle info, object param)
        {
            throw new NotImplementedException();
        }

        public bool CheckFinish(IConditionHandle info)
        {
            throw new NotImplementedException();
        }

        public void OnEventTrigger(object param)
        {

        }
    }

    public class Con2Compare : IConditionCompare
    {
        int IConditionCompare.Target => CondConst.GEM;

        public bool Check(IConditionHandle info, object param)
        {
            throw new NotImplementedException();
        }

        public bool CheckFinish(IConditionHandle info)
        {
            throw new NotImplementedException();
        }

        public void OnEventTrigger(object param)
        {

        }
    }

    public class Con3Compare : IConditionCompare
    {
        int IConditionCompare.Target => CondConst.TEST;

        public bool Check(IConditionHandle info, object param)
        {
            throw new NotImplementedException();
        }

        public bool CheckFinish(IConditionHandle info)
        {
            throw new NotImplementedException();
        }

        public void OnEventTrigger(object param)
        {

        }
    }
}

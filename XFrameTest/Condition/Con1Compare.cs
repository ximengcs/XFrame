using XFrame.Modules.Conditions;

namespace XFrameTest.Condition
{
    public class Con1Compare : IConditionCompare
    {
        int IConditionCompare.Target => CondConst.COIN;

        bool IConditionCompare.Check(ConditionHandle info, object param)
        {
            return false;
        }

        bool IConditionCompare.CheckFinish(ConditionHandle info)
        {
            return false;
        }
    }

    public class Con2Compare : IConditionCompare
    {
        int IConditionCompare.Target => CondConst.GEM;

        bool IConditionCompare.Check(ConditionHandle info, object param)
        {
            return true;
        }

        bool IConditionCompare.CheckFinish(ConditionHandle info)
        {
            return true;
        }
    }

    public class Con3Compare : IConditionCompare
    {
        int IConditionCompare.Target => CondConst.TEST;

        bool IConditionCompare.Check(ConditionHandle info, object param)
        {
            return true;
        }

        bool IConditionCompare.CheckFinish(ConditionHandle info)
        {
            return true;
        }
    }
}

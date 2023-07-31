using XFrame.Modules.Conditions;
using XFrame.Modules.Pools;

namespace XFrameTest.Condition
{
    public class Con1Compare : PoolObjectBase, IConditionCompare<int>
    {
        int IConditionCompare.Target => 1;

        bool IConditionCompare<int>.Check(IConditionHandle info, int param)
        {
            Console.WriteLine("param " + param);
            return true;
        }

        bool IConditionCompare.CheckFinish(IConditionHandle info)
        {
            Console.WriteLine("CheckFinish ");
            return true;
        }

        void IConditionCompare<int>.OnEventTrigger(int param)
        {
            Console.WriteLine("OnEventTrigger " + param);
        }
    }

}

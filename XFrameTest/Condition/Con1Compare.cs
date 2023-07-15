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

    public class Con2Compare : IConditionCompare
    {
        int IConditionCompare.Target => throw new NotImplementedException();

        int IPoolObject.PoolKey => throw new NotImplementedException();

        bool IConditionCompare.CheckFinish(IConditionHandle info)
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

    public class Con3Compare : IConditionCompare
    {
        int IConditionCompare.Target => throw new NotImplementedException();

        int IPoolObject.PoolKey => throw new NotImplementedException();

        bool IConditionCompare.CheckFinish(IConditionHandle info)
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

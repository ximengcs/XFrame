using XFrame.Core;
using XFrame.Modules.Conditions;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Pools;

namespace XFrameTest.Condition
{
    public class Con1Compare : IConditionCompare
    {
        public int PoolKey => throw new NotImplementedException();

        int IConditionCompare.Target => CondConst.COIN;

        int IPoolObject.PoolKey => throw new NotImplementedException();

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

        bool IConditionCompare.Check(IConditionHandle info, object param)
        {
            throw new NotImplementedException();
        }

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

        void IConditionCompare.OnEventTrigger(object param)
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

    public class Con2Compare : IConditionCompare
    {
        public int PoolKey => throw new NotImplementedException();

        int IConditionCompare.Target => CondConst.GEM;

        int IPoolObject.PoolKey => throw new NotImplementedException();

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

        bool IConditionCompare.Check(IConditionHandle info, object param)
        {
            throw new NotImplementedException();
        }

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

        void IConditionCompare.OnEventTrigger(object param)
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
        public int PoolKey => throw new NotImplementedException();

        int IConditionCompare.Target => CondConst.TEST;

        int IPoolObject.PoolKey => throw new NotImplementedException();

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

        bool IConditionCompare.Check(IConditionHandle info, object param)
        {
            throw new NotImplementedException();
        }

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

        void IConditionCompare.OnEventTrigger(object param)
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

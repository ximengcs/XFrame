using XFrame.Modules.Pools;

namespace XFrameTest
{
    public class PoolObj1 : IPoolObject
    {
        void IPoolObject.OnCreate()
        {
            throw new NotImplementedException();
        }

        void IPoolObject.OnRelease()
        {
            throw new NotImplementedException();
        }

        void IPoolObject.OnDestroyForever()
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class PoolTest
    {
    }
}

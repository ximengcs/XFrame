using XFrame.Modules;

namespace XFrameTest
{
    public class PoolObj1 : IPoolObject
    {
        public void OnCreate()
        {
            Console.WriteLine("OnCreate " + GetHashCode());
        }

        public void OnDestroyFrom()
        {
            Console.WriteLine("OnDestroyFrom " + GetHashCode());
        }

        public void OnRelease()
        {
            Console.WriteLine("OnRelease " + GetHashCode());
        }
    }

    [TestClass]
    public class PoolTest
    {
        [TestMethod]
        public void Test1()
        {
            new PoolModule().OnInit(null);
            IPoolSystem<PoolObj1> poolSystem = PoolModule.Inst.GetOrNew<PoolObj1>();
            IPool<PoolObj1> pool1 = poolSystem.Require<PoolObj1>();
            IPool<PoolObj1> pool2 = poolSystem.Require<PoolObj1>();
            IPool<PoolObj1> pool3 = poolSystem.Require<PoolObj1>();
            IPool<PoolObj1> pool4 = poolSystem.Require<PoolObj1>();
            IPool<PoolObj1> pool5 = poolSystem.Require<PoolObj1>();

            pool1.Require(out IPoolObject obj1);
            pool2.Require(out IPoolObject obj2);
            pool3.Require(out IPoolObject obj3);
            pool4.Require(out IPoolObject obj4);
            pool5.Require(out IPoolObject obj5);

            Debug(pool1);
            pool1.Release(obj2);
            Debug(pool1);
            pool1.Release(obj3);
            Debug(pool1);
            pool1.Release(obj4);
            Debug(pool1);
            pool1.Release(obj5);
            Debug(pool1);
            Console.WriteLine("444");
            Debug(pool4);
            pool4.Require(out obj5);
            Debug(pool4);
            pool4.Require(out obj4);
            Debug(pool4);
            Console.WriteLine("555");
            Debug(pool5);
            pool5.Release(obj5);
            Debug(pool5);
            pool5.Release(obj4);
            Debug(pool5);
        }

        private void Debug(IPool<PoolObj1> pool)
        {
            Console.WriteLine("====");
            Console.WriteLine(pool.ToString());
            Console.WriteLine("====");
        }
    }
}

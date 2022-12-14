using XFrame.Modules;

namespace XFrameTest
{
    [TestClass]
    public class TypeTest
    {
        public void Test1()
        {
            Type poolObjType = typeof(Entity);
            Type poolType = typeof(ObjectPool<>).MakeGenericType(poolObjType);
            IPool pool = Activator.CreateInstance(poolType, 8) as IPool;
        }
    }
}

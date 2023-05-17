using XFrame.Collections;
using XFrame.Core;
using XFrame.Modules.Config;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Pools;

namespace XFrameTest
{
    public class TestLog : ILogger
    {
        public void Debug(params object[] content)
        {
            Console.WriteLine(content[0]);
        }

        public void Error(params object[] content)
        {
            Console.WriteLine(content);
        }

        public void Fatal(params object[] content)
        {
            Console.WriteLine(content);
        }

        public void Warning(params object[] content)
        {
            Console.WriteLine(content);
        }
    }

    public class PoolObj1 : IPoolObject
    {
        public int Name;

        int IPoolObject.PoolKey => 0;

        void IPoolObject.OnCreate()
        {
            Log.Debug($"OnCreate{Name}");
        }

        void IPoolObject.OnRequest()
        {
            Log.Debug($"OnRequest{Name}");
        }

        void IPoolObject.OnRelease()
        {
            Log.Debug($"OnRelease{Name}");
        }

        void IPoolObject.OnDelete()
        {
            Log.Debug($"OnDelete{Name}");
        }
    }

    [TestClass]
    public class PoolTest
    {
        [TestMethod]
        public void Test()
        {

        }
    }
}

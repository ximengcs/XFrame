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

    public class PoolObj1 : PoolObjectBase, IPoolObject
    {
        public int Name;
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

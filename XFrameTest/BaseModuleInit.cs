using XFrame.Core;

namespace XFrameTest
{
    [TestClass]
    public class BaseModuleInit
    {
        [TestMethod]
        public void Test1()
        {
            Entry.Init();
            Entry.ShutDown();
        }
    }
}

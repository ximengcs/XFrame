using XFrame.Collections;
using XFrame.Core;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Pools;
using XFrame.Modules.Rand;

namespace XFrameTest
{
    [TestClass]
    public class RandomTest
    {
        enum TestEnum
        {
            A,
            B = 5,
            C
        }

        [TestMethod]
        public void Test1()
        {
            EntryTest.Exec(() =>
            {
                References.Require<XLinkList<int>>();
                Log.Debug(XModule.Rand.RandString());
                Log.Debug(XModule.Rand.RandString());
                Log.Debug(XModule.Rand.RandString());
                Log.Debug(XModule.Rand.RandString());
                Log.Debug(XModule.Rand.RandString());
                Log.Debug(XModule.Rand.RandString());
                Log.Debug(XModule.Rand.RandEnum<TestEnum>());
                Log.Debug(XModule.Rand.RandEnum<TestEnum>());
                Log.Debug(XModule.Rand.RandEnum<TestEnum>());
                Log.Debug(XModule.Rand.RandEnum<TestEnum>());
                Log.Debug(XModule.Rand.RandEnum<TestEnum>());
                Log.Debug(XModule.Rand.RandEnum<TestEnum>());
                Log.Debug(XModule.Rand.RandEnum<TestEnum>());
            });
        }
    }
}

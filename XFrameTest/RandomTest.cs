using XFrame.Module.Rand;
using XFrame.Modules.Diagnotics;

namespace XFrameTest
{
    [TestClass]
    public class RandomTest
    {
        [TestMethod]
        public void Test1()
        {
            EntryTest.Exec(() =>
            {
                Log.Debug(RandModule.Inst.RandString());
                Log.Debug(RandModule.Inst.RandString());
                Log.Debug(RandModule.Inst.RandString());
                Log.Debug(RandModule.Inst.RandString());
                Log.Debug(RandModule.Inst.RandString());
                Log.Debug(RandModule.Inst.RandString());
            });
        }
    }
}

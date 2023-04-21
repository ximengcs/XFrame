using XFrame.Module.Rand;
using XFrame.Modules.Diagnotics;

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
                Log.Debug(RandModule.Inst.RandString());
                Log.Debug(RandModule.Inst.RandString());
                Log.Debug(RandModule.Inst.RandString());
                Log.Debug(RandModule.Inst.RandString());
                Log.Debug(RandModule.Inst.RandString());
                Log.Debug(RandModule.Inst.RandString());
                Log.Debug(RandModule.Inst.RandEnum<TestEnum>());
                Log.Debug(RandModule.Inst.RandEnum<TestEnum>());
                Log.Debug(RandModule.Inst.RandEnum<TestEnum>());
                Log.Debug(RandModule.Inst.RandEnum<TestEnum>());
                Log.Debug(RandModule.Inst.RandEnum<TestEnum>());
                Log.Debug(RandModule.Inst.RandEnum<TestEnum>());
                Log.Debug(RandModule.Inst.RandEnum<TestEnum>());
            });
        }
    }
}

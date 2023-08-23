
using XFrame.Core;
using XFrame.Modules.Diagnotics;

namespace XFrameTest
{
    [TestClass]
    public class CoreTest
    {
        public class A : ModuleBase, IUpdater
        {
            public void OnUpdate(float escapeTime)
            {
                Log.Debug("A Update");
            }
        }

        [XModule]
        public class B : SingletonModule<B>, IUpdater
        {
            public void OnUpdate(float escapeTime)
            {
                Log.Debug("B Update");
            }
        }

        [TestMethod]
        public void Test()
        {
            EntryTest.Exec(() =>
            {
                Entry.AddModule<A>();
                Log.ToQueue = false;
                Log.Debug("Start");
            });
        }
    }
}

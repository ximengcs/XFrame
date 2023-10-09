
using XFrame.Core;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Reflection;

namespace XFrameTest
{
    [TestClass]
    public class CoreTest
    {
        public class A : ModuleBase, IUpdater
        {
            public void OnUpdate(float escapeTime)
            {
                
            }
        }

        [CommonModule]
        public class B : SingletonModule<B>, IUpdater
        {
            public void OnUpdate(float escapeTime)
            {
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
                Log.Debug(Entry.GetModule<ITypeModule>() == null);
            });
        }

        [TestMethod]
        public void Test2()
        {
            EntryTest.Exec(() =>
            {
                Log.ConsumeWaitQueue();
                Log.ToQueue = false;
            });
        }
    }
}

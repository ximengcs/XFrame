﻿
using XFrame.Core;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.XType;

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
    }
}

using XFrame.Core;
using XFrame.Modules.Config;
using XFrameTest.Common;

namespace XFrameTest
{
    public class EntryTest
    {
        public static void Exec(Action runCallback)
        {
            Exec(9999, runCallback);
        }

        public static void Exec(int times, Action runCallback)
        {
            XConfig.ArchiveEncrypt = false;
            XConfig.TypeChecker = new DefaultTypeChecker();
            XConfig.DefaultLogger = typeof(ConsoleLogger).FullName;
            XConfig.ArchivePath = "D:\\TestXFrame";
            XConfig.DefaultDownloadHelper = typeof(TestDownloadHelper).FullName;

            Entry.Init();
            Entry.OnRun += runCallback;
            Entry.Start();
            for (int i = 0; i < times; i++)
                Entry.Trigger<IUpdater>(0.16f);
            Entry.ShutDown();
        }
    }
}

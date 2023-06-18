using XFrame.Core;
using XFrame.Modules.Config;

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
            XConfig.UseClassModule = new string[] { "XFrameTest" };
            XConfig.DefaultSerializer = typeof(TestJsonSerializer).FullName;
            XConfig.DefaultLogger = typeof(ConsoleLogger).FullName;
            XConfig.ArchivePath = "D:\\TestXFrame";

            Entry.Init();
            Entry.OnRun += runCallback;
            Entry.Start();
            for (int i = 0; i < times; i++)
                Entry.Update(0.16f);
            Entry.ShutDown();
        }
    }
}

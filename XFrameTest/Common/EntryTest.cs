using XFrame.Core;

namespace XFrameTest
{
    public class EntryTest
    {
        public static void Exec(Action runCallback)
        {
            Entry.Init();
            Entry.OnRun += runCallback;
            Entry.Start();
            for (int i = 0; i < 9999; i++)
                Entry.Update(0.16f);
            Entry.ShutDown();
        }
    }
}

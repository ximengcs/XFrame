using System.Threading;

namespace XFrame.Core.Threads
{
    public static class FiberExtension
    {
        public static void StartThread(this Fiber param)
        {
            Thread thread = new Thread((p) =>
            {
                Fiber fiber = p as Fiber;
                fiber.Use();
                while (true)
                {
                    fiber.Update(0);
                    Thread.Sleep(0);
                }
            });
            thread.Start(param);
            param.SetThread(thread);
        }
    }
}

using System;
using System.Threading;
using System.Timers;
using XFrame.Modules.Diagnotics;

namespace XFrame.Core.Threads
{
    public static class FiberExtension
    {
        public static Action SleepBefore;
        public static Action SleepAfter;

        public static void Sleep(int millisecondsTimeout)
        {
            SleepBefore?.Invoke();
            Thread.Sleep(millisecondsTimeout);
            SleepAfter?.Invoke();
        }

        private static void ThreadWaitCallback(object state)
        {
            ValueTuple<Fiber, int> data = (ValueTuple<Fiber, int>)state;
            int time = data.Item2;
            Fiber fiber = data.Item1;
            fiber.SetThread(Thread.CurrentThread.ManagedThreadId);
            fiber.Use();
            while (!fiber.Disposed)
            {
                try
                {
                    fiber.Update(time);
                    Sleep(time);
                }
                catch (Exception ex)
                {
                    Log.Exception(ex);
                }
            }
        }

        public static void StartThread(this Fiber param, int waitMillSecondTime = 0)
        {
            ThreadPool.QueueUserWorkItem(ThreadWaitCallback, ValueTuple.Create(param, waitMillSecondTime));
        }
    }
}

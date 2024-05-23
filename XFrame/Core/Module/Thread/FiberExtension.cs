﻿using System;
using System.Threading;

namespace XFrame.Core.Threads
{
    public static class FiberExtension
    {
        public static void StartThread(this Fiber param, int waitMillSecondTime = 0)
        {
            Thread thread = new Thread((p) =>
            {
                ValueTuple<Fiber, int> data = (ValueTuple<Fiber, int>)p;
                int time = data.Item2;
                Fiber fiber = data.Item1;
                fiber.Use();
                while (true)
                {
                    fiber.Update(time);
                    Thread.Sleep(time);
                }
            });
            thread.Start(ValueTuple.Create(param, waitMillSecondTime));
            param.SetThread(thread);
        }
    }
}

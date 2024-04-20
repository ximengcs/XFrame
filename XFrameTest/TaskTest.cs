using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XFrame.Core;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Download;
using XFrame.Modules.Tasks;
using XFrame.Modules.Threads;
using XFrame.Modules.Times;
using XFrame.Tasks;

namespace XFrameTest
{
    [TestClass]
    public class TaskTest
    {
        [TestMethod]
        public void Test()
        {
            EntryTest.Exec(() =>
            {
                Log.ToQueue = false;
                Log.Debug($"[{XModule.Time.Frame}]");

                /*
                ActionTask task = XModule.Task.GetOrNew<ActionTask>();
                task.Add(0, () =>
                {
                    Log.Debug($"[{XModule.Time.Frame}] Exec0");
                });
                task.Add(() =>
                {
                    Log.Debug($"[{XModule.Time.Frame}] Exec1");
                });
                task.Add(() =>
                {
                    Log.Debug($"[{XModule.Time.Frame}] Exec2");
                });
                bool first = false;
                task.Add(() =>
                {
                    Log.Debug($"[{XModule.Time.Frame}] Exec3");
                    bool value = first;
                    first = true;
                    return value;
                });
                float pro = 0;
                task.Add(() =>
                {
                    //if (pro % 10 == 0)
                    Log.Debug($"[{XModule.Time.Frame}] Exec4 {pro}");
                    pro++;
                    return pro / 10;
                });
                task.Add(0, () =>
                {
                    Log.Debug($"[{XModule.Time.Frame}] Exec5");
                });
                int repeatTimes = 0;
                task.Add(0.1f, () =>
                {
                    Log.Debug($"[{XModule.Time.Frame}] Exec6 repeat {repeatTimes}");
                    repeatTimes++;
                    return repeatTimes > 5;
                });
                task.Add(() =>
                {
                    Log.Debug($"[{XModule.Time.Frame}] Exec7");
                }, true);
                task.Add(() =>
                {
                    Log.Debug($"[{XModule.Time.Frame}] Exec8");
                }, true);
                task.OnComplete(() =>
                {
                    Log.Debug($"[{XModule.Time.Frame}] Complete {task.GetHashCode()}");
                }).Start();

                DelayTask delay = TaskModule.Inst.GetOrNew<DelayTask>();
                delay.Add(1.0f, () =>
                {
                    Log.Debug($"[{TimeModule.Inst.Frame}] Delay complete {task.GetHashCode()}");
                    ActionTask t = TaskModule.Inst.GetOrNew<ActionTask>();
                    t.Add(() => Log.Debug($"[{TimeModule.Inst.Frame}] Exec3"));
                    t.Add(() => Log.Debug($"[{TimeModule.Inst.Frame}] Exec4"));
                    t.OnComplete(() =>
                    {
                        Log.Debug($"[{TimeModule.Inst.Frame}] Complete2 {task.GetHashCode()}");
                    }).Start();
                }).Start();*/
            });
        }

        [TestMethod]
        public void TestDownload()
        {
            EntryTest.Exec(async () =>
            {
                string url = "https://c1b.tapque.com/Innovate/ASMRMakeOver/Production/iOS/V1.0.8/ASMRMakeOverConfig.json";
                Console.WriteLine("Download Start");
                DownTask task = await XModule.Download.Down(url);
                Console.WriteLine("Download Complete");
                Console.WriteLine(task.Text);
            });
        }

        [TestMethod]
        public void TestStartup()
        {
            EntryTest.Exec(() => { Log.ConsumeWaitQueue(); });
        }

        [TestMethod]
        public void TestDelay()
        {
            EntryTest.Exec(async () =>
            {
                Console.WriteLine("before");
                await XTask.Delay(1000);
                Console.WriteLine("after");
            });
        }
    }
}

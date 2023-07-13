using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Download;
using XFrame.Modules.Tasks;
using XFrame.Modules.Threads;
using XFrame.Modules.Times;

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
                Log.Debug($"[{TimeModule.Inst.Frame}]");
                ActionTask task = TaskModule.Inst.GetOrNew<ActionTask>();
                task.Add(() =>
                {
                    Log.Debug($"[{TimeModule.Inst.Frame}] Exec1");
                });
                task.Add(() =>
                {
                    Log.Debug($"[{TimeModule.Inst.Frame}] Exec2");
                });
                task.OnComplete(() =>
                {
                    Log.Debug($"[{TimeModule.Inst.Frame}] Complete {task.GetHashCode()}");
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
                }).Start();
            });
        }

        [TestMethod]
        public void TestDownload()
        {
            EntryTest.Exec(() =>
            {
                string url = "https://c1b.tapque.com/Innovate/ASMRMakeOver/Production/iOS/V1.0.8/ASMRMakeOverConfig.json";
                DownloadModule.Inst.Down(url).OnComplete((string text) =>
                {
                    Console.WriteLine("Download Complete");
                    Console.WriteLine(text);
                }).Start();
            });
        }

        [TestMethod]
        public void TestStartup()
        {
            EntryTest.Exec(() => { Log.ConsumeWaitQueue(); });
        }
    }
}

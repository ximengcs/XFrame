using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Tasks;
using XFrame.Modules.Threads;

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
                ActionTask task = TaskModule.Inst.GetOrNew<ActionTask>();
                task.Add(() => Log.Debug("Exec1"));
                task.Add(() => Log.Debug("Exec2"));
                task.OnComplete(() =>
                {
                    Log.Debug($"Complete {task.GetHashCode()}");
                }).Start();

                DelayTask delay = TaskModule.Inst.GetOrNew<DelayTask>();
                delay.Add(1.0f, () =>
                {
                    Log.Debug($"Delay complete {task.GetHashCode()}");
                    ActionTask t = TaskModule.Inst.GetOrNew<ActionTask>();
                    t.Add(() => Log.Debug("Exec3"));
                    t.Add(() => Log.Debug("Exec4"));
                    t.OnComplete(() =>
                    {
                        Log.Debug($"Complete2 {task.GetHashCode()}");
                    }).Start();
                }).Start();
            });
        }
    }
}

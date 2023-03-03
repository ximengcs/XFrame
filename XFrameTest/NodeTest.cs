﻿using XFrame.Core;
using XFrame.Module.Rand;
using XFrame.Modules.Tasks;
using XFrame.Modules.Times;
using XFrame.Modules.XType;

namespace XFrameTest
{
    [TestClass]
    public class NodeTest
    {
        [TestMethod]
        public void Test1()
        {
            XCore core = XCore.Create(
                typeof(RandModule),
                typeof(TypeModule),
                typeof(TimeModule),
                typeof(TaskModule));

            float pro = 0;
            var task = TaskModule.Inst.GetOrNew<ProActionTask>();
            task.Add(() =>
            {
                pro += 0.02f;
                return pro;
            }).OnUpdate((pro) =>
            {
                Console.WriteLine(pro);
            }).OnComplete(() =>
            {
                Console.WriteLine("Complete");
            });
            task.Coroutine();

            for (int i = 0; i < 100; i++)
            {
                core.Update(1);
            }
        }
    }
}

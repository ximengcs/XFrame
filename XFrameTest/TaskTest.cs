using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFrame.Modules.Threads;

namespace XFrameTest
{
    [TestClass]
    public class TaskTest
    {
        [TestMethod]
        public void Test()
        {
            Task task = new Task(() =>
            {
                for (int i = 0; i < 100; i++)
                    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} 1 " + i);
            });

            task.Start();
            for (int i = 0; i < 100; i++)
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} 2 " + i);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFrame.Core.Binder;

namespace XFrameTest.Test
{
    [TestClass]
    public class ListTest
    {
        [TestMethod]
        public void Test()
        {
            List_<int> list = new List_<int>();
            TriggerBinder<List_<int>> binder = new TriggerBinder<List_<int>>(() => list);
            binder.AddHandler((list) =>
            {
                Console.WriteLine("Value Change");
                Console.WriteLine(list);
            });


            list.Add(1);
            list.Add(2);
            list.Remove(2);
        }
    }
}

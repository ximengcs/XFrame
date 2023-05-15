using XFrame.Core.Binder;
using XFrame.Modules.Download;

namespace XFrameTest
{
    [TestClass]
    public class ValueBinderTest
    {
        [TestMethod]
        public void Test0()
        {
            Func<bool> fun = () =>
            {
                Console.WriteLine("true");
                return true;
            };
            fun += () => false;
            Console.WriteLine(fun());

        }

        public void T(Action a)
        {
            a += () =>
            {
                Console.WriteLine("3");
            };
        }


        [TestMethod]
        public void Test1()
        {
            int value = default;
            ValueBinder<int> test = new ValueBinder<int>(() => value, (v) => value = v);
            Action<int> handler = (v) =>
            {
                Console.WriteLine("Value Change " + v);
            };
            test.AddHandler(handler);
            test.AddCondHandler((v) =>
            {
                Console.WriteLine("Value Change once " + v);
                return true;
            });
            test.AddCondHandler((v) =>
            {
                Console.WriteLine("Value Change once 2 " + v);
                return false;
            });

            test.Value = 1;
            test.RemoveHandler(handler);
            test.Value = 2;
        }
    }
}
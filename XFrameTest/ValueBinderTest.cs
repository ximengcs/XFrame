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

    }
}
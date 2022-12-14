using XFrame.Core;

namespace XFrameTest
{
    [TestClass]
    public class ValueBinderTest
    {
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
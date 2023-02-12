using XFrame.Collections;

namespace XFrameTest
{
    [TestClass]
    public class CollectionTest
    {
        [TestMethod]
        public void Test1()
        {
            XLinkList<string> list = new XLinkList<string>(false);
            list.AddLast("1");
            list.AddLast("2");
            list.AddLast("3");
            list.AddLast("4");
            list.AddLast("5");

            list.SetIt(XItType.Backward);
            foreach (string item in list)
            {
                Console.WriteLine(item);
            }
        }
    }
}

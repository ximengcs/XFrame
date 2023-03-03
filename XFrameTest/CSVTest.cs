using XFrame.Collections;

namespace XFrameTest
{
    [TestClass]
    public class CSVTest
    {
        [TestMethod]
        public void Test1()
        {
            string csvFile = "1,2,3\n4,5,6";
            Csv csv = new Csv(csvFile);
            Console.Write("Row " + csv.Row);
            foreach (var line in csv)
            {
                Console.Write("\nLine Count " + line.Count + " -> ");
                foreach (string content in line)
                {
                    Console.Write(content + " ");
                }
            }
        }
    }
}

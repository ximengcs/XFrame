
using System.Text;
using XFrame.Collections;

namespace XFrameTest
{
    [TestClass]
    public class CSVTest
    {
        [TestMethod]
        public void Test1()
        {
            string csvFile = File.ReadAllText($"C:/Users/98259/Desktop/1.csv");
            //Console.WriteLine(csvFile);
            Csv csv = new Csv(csvFile);
            Console.WriteLine(csv.Row);
            for (int row = 0; row < csv.Row; row++)
            {
                for (int column = 0; column < csv.Column; column++)
                {
                    string cont = csv.Get(row, column);
                    Console.Write(cont + $" {cont.Length}   ");
                }
                Console.WriteLine();
            }
        }
    }
}

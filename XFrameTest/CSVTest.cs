using CsvHelper;
using System.Globalization;
using XFrame.Collections;
using XFrame.Core;
using XFrame.Modules.Pools;

namespace XFrameTest
{
    [TestClass]
    public class CSVTest
    {
        [TestMethod]
        public void Test3()
        {
            EntryTest.Exec(() =>
            {
                string text = ",1,,2,3,4";
                Csv<string> csv = new Csv<string>(text, References.Require<StringParser>());
                Console.WriteLine(csv.ToString());
            });
        }

        [TestMethod]
        public void Test2()
        {
            EntryTest.Exec(() =>
            {
                Csv<string> csv = new Csv<string>();
                Csv<string>.Line line1 = csv.Add();
                line1[0] = "0"; line1[2] = "2"; line1[3] = "3";

                Csv<string>.Line line2 = csv.Add();
                line2[0] = "20"; line2[2] = "22"; line2[3] = "23";
                Console.WriteLine(csv);

                Csv<string> csv3 = new Csv<string>(csv.ToString(), References.Require<StringParser>());
                Console.WriteLine(csv3.ToString());
            });
        }

        [TestMethod]
        public void Test1()
        {
            EntryTest.Exec(() =>
            {
                string csvFile = "1,2,3\n4,5,6";
                Csv<string> csv = new Csv<string>(csvFile, References.Require<StringParser>());
                Console.Write("Row " + csv.Row);
                foreach (var line in csv)
                {
                    Console.Write("\nLine Count " + line.Count + " -> ");
                    foreach (string content in line)
                    {
                        Console.Write(content + " ");
                    }
                }
            });
        }

        [TestMethod]
        public void Test4()
        {
            string text = File.ReadAllText("C:\\Users\\XM\\Desktop\\debug.txt");
            CultureInfo[] infos = CultureInfo.GetCultures(CultureTypes.AllCultures);
            CultureInfo german = null;
            int count1 = 0;
            foreach (CultureInfo info in infos)
            {
                bool exc = false;
                try
                {
                    CsvReader csvReader = new CsvReader(new StringReader(text), CultureInfo.InvariantCulture);
                    int row = 0;
                    while (csvReader.Read())
                    {
                        //Console.WriteLine("row " + row++);
                        int count = csvReader.Parser.Count;
                        for (int i = 0; i < count; i++)
                        {
                            //Console.WriteLine("=============");
                            //Console.Write($"[{csvReader[i]}]");
                            //Console.WriteLine("\n=============");
                        }
                    }
                }
                catch (Exception e)
                {
                    count1++;
                    Console.WriteLine(info.EnglishName);
                    exc = true;
                }
                if (exc)
                {
                    //Console.WriteLine("normal " + info.EnglishName);
                }
            }
        }

        [TestMethod]
        public void Test5()
        {

        }
    }
}

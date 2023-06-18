﻿using XFrame.Collections;
using XFrame.Core;

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
                Csv<string> csv = new Csv<string>(text, ParserModule.Inst.STRING);
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

                Csv<string> csv3 = new Csv<string>(csv.ToString(), ParserModule.Inst.STRING);
                Console.WriteLine(csv3.ToString());
            });
        }

        [TestMethod]
        public void Test1()
        {
            EntryTest.Exec(() =>
            {
                string csvFile = "1,2,3\n4,5,6";
                Csv<string> csv = new Csv<string>(csvFile, ParserModule.Inst.STRING);
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
    }
}

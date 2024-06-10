using System.Text;
using XFrame.Collections;
using XFrame.Modules.Config;
using XFrame.Modules.Archives;
using XFrame.Modules.Diagnotics;
using XFrame.Core;

namespace XFrameTest
{
    public class D1
    {
        public int Index = 222;
        public string Name = "111";

        public override string ToString()
        {
            return $"{Index} {Name}";
        }
    }

    [TestClass]
    public class ArchiveTest
    {
        [TestMethod]
        public void Test1()
        {
            EntryTest.Exec(() =>
            {
                JsonArchive archive = Entry.GetModule<ArchiveModule>().GetOrNew<JsonArchive>("archive_test");
                archive.Set("name", "simon");

                IJsonArchive a1 = archive.SpwanDataProvider("a1");
                a1.SetData(1);
                a1.SetData(1.2);

                IJsonArchive a2 = archive.SpwanDataProvider("a2");
                a2.SetData("1");
                a2.SetData(new D1());
            });
        }

        [TestMethod]
        public void Test3()
        {
            EntryTest.Exec(() =>
            {
                JsonArchive archive = Entry.GetModule<ArchiveModule>().GetOrNew<JsonArchive>("archive_test");
                Console.WriteLine(archive.Get<string>("name"));

                IJsonArchive a1 = archive.SpwanDataProvider("a1");
                Console.WriteLine(a1.GetData<int>());
                Console.WriteLine(a1.GetData<double>());

                IJsonArchive a2 = archive.SpwanDataProvider("a2");
                Console.WriteLine(a2.GetData<string>());
                Console.WriteLine(a2.GetData<D1>());
            });
        }

        [TestMethod]
        public void Test2()
        {
            EntryTest.Exec(() =>
            {
                JsonArchive archive = Entry.GetModule<ArchiveModule>().GetOrNew<JsonArchive>("archive_test");
                Log.Debug(archive.Get<string>("name"));
            });
        }

        [TestMethod]
        public void Test11()
        {
            EntryTest.Exec(() =>
            {
                CsvArchive csv = Entry.GetModule<ArchiveModule>().GetOrNew<CsvArchive>("archive_csv_test", 3);
                Csv<string>.Line line = csv.Data.Add();
                line[0] = "a";
                line[2] = "b";
            });
        }

        [TestMethod]
        public void Test4()
        {
            EntryTest.Exec(() =>
            {
                CsvArchive csv = Entry.GetModule<ArchiveModule>().GetOrNew<CsvArchive>("archive_csv_test");
                Log.Debug(csv.Data);
            });
        }

        [TestMethod]
        public void Test5()
        {
            EntryTest.Exec(() =>
            {
                DataArchive archive = Entry.GetModule<ArchiveModule>().GetOrNew<DataArchive>("archive_bin_test");
                archive.Write("a1.txt", Encoding.UTF8.GetBytes("testa1"));
            });
        }

        [TestMethod]
        public void Test6()
        {
            EntryTest.Exec(() =>
            {
                DataArchive archive = Entry.GetModule<ArchiveModule>().GetOrNew<DataArchive>("archive_bin_test");
                archive.ExportDisk(XConfig.ArchivePath);
            });
        }
    }
}

using System.Text;
using XFrame.Collections;
using XFrame.Modules.Config;
using XFrame.Modules.Archives;
using XFrame.Modules.Diagnotics;

namespace XFrameTest
{
    [TestClass]
    public class ArchiveTest
    {
        [TestMethod]
        public void Test1()
        {
            EntryTest.Exec(() =>
            {
                JsonArchive archive = ArchiveModule.Inst.GetOrNew<JsonArchive>("archive_test");
                archive.Set("name", "simon");
            });
        }

        [TestMethod]
        public void Test2()
        {
            EntryTest.Exec(() =>
            {
                JsonArchive archive = ArchiveModule.Inst.GetOrNew<JsonArchive>("archive_test");
                Log.Debug(archive.Get<string>("name"));
            });
        }

        [TestMethod]
        public void Test3()
        {
            EntryTest.Exec(() =>
            {
                CsvArchive csv = ArchiveModule.Inst.GetOrNew<CsvArchive>("archive_csv_test", 3);
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
                CsvArchive csv = ArchiveModule.Inst.GetOrNew<CsvArchive>("archive_csv_test");
                Log.Debug(csv.Data);
            });
        }

        [TestMethod]
        public void Test5()
        {
            EntryTest.Exec(() =>
            {
                DataArchive archive = ArchiveModule.Inst.GetOrNew<DataArchive>("archive_bin_test");
                archive.Write("a1.txt", Encoding.UTF8.GetBytes("testa1"));
            });
        }

        [TestMethod]
        public void Test6()
        {
            EntryTest.Exec(() =>
            {
                DataArchive archive = ArchiveModule.Inst.GetOrNew<DataArchive>("archive_bin_test");
                archive.ExportDisk(XConfig.ArchivePath);
            });
        }
    }
}

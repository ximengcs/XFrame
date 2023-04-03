
using XFrame.Modules.Archives;

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
    }
}

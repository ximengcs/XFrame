
using XFrame.Collections;
using XFrame.Modules.Archives;
using XFrame.Modules.Config;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Local;

namespace XFrameTest
{
    [TestClass]
    public class LocalizeTest
    {
        [TestMethod]
        public void TestCreate()
        {
            //测试前关闭加密
            EntryTest.Exec(() =>
            {
                ArchiveModule.Inst.Delete("lang");
                CsvArchive archive = ArchiveModule.Inst.GetOrNew<CsvArchive>("lang");
                Csv<string>.Line line = archive.Data.Add();
                line[0] = "1"; line[1] = "English"; line[2] = "ChineseSimplified";

                line = archive.Data.Add();
                line[0] = "2"; line[1] = "Test"; line[2] = "测试";

                line = archive.Data.Add();
                line[0] = "3"; line[1] = "Test2"; line[2] = "测试2";

                line = archive.Data.Add();
                line[0] = "4"; line[1] = "Test2_{0}_{1}_{2}"; line[2] = "测试2_{0}_{1}_{2:name}_{2}";

                Log.Debug(archive.Data.ToString());
                archive.Save();
            });
        }

        class IdFormatter : ICustomFormatter
        {
            public string Format(string? format, object? arg, IFormatProvider? formatProvider)
            {
                return format;
            }
        }

        [TestMethod]
        public void Test1()
        {
            EntryTest.Exec(() =>
            {
                LocalizeModule.Inst.SetFormater(new IdFormatter());
                LocalizeModule.Inst.Lang = Language.ChineseSimplified;
                Log.Debug($"{LocalizeModule.Inst.GetValueParam(4, 1, 2, 3)}");
                Log.Debug($"{LocalizeModule.Inst.GetValue(4, "q", "w", "e")}");

                LocalizeModule.Inst.Lang = Language.English;
                Log.Debug($"{LocalizeModule.Inst.GetValue(4, "q", "w", "e")}");
            });
        }
    }
}

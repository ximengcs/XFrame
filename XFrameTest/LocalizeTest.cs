
using XFrame.Collections;
using XFrame.Core;
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
                Entry.GetModule<ArchiveModule>().Delete("lang");
                CsvArchive archive = Entry.GetModule<ArchiveModule>().GetOrNew<CsvArchive>("lang");
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
            public string Format(string format, object arg, IFormatProvider? formatProvider)
            {
                return format;
            }
        }

        [TestMethod]
        public void Test1()
        {
            EntryTest.Exec(() =>
            {
                Log.ToQueue = false;
                Entry.GetModule<ILocalizeModule>().SetFormater(new IdFormatter());
                Entry.GetModule<ILocalizeModule>().Lang = Language.ChineseSimplified;
                Log.Debug($"{Entry.GetModule<ILocalizeModule>().GetValueParam(3, 0, 1, 2)}");
                Log.Debug($"{Entry.GetModule<ILocalizeModule>().GetValue(3, "q", "w", "e")}");

                Entry.GetModule<ILocalizeModule>().Lang = Language.English;
                Log.Debug($"{Entry.GetModule<ILocalizeModule>().GetValue(3, "q", "w", "e")}");
            });
        }
    }
}

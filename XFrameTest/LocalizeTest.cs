
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
                ModuleUtility.Archive.Delete("lang");
                CsvArchive archive = ModuleUtility.Archive.GetOrNew<CsvArchive>("lang");
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
                ModuleUtility.Local.SetFormater(new IdFormatter());
                ModuleUtility.Local.Lang = Language.ChineseSimplified;
                Log.Debug($"{ModuleUtility.Local.GetValueParam(3, 0, 1, 2)}");
                Log.Debug($"{ModuleUtility.Local.GetValue(3, "q", "w", "e")}");

                ModuleUtility.Local.Lang = Language.English;
                Log.Debug($"{ModuleUtility.Local.GetValue(3, "q", "w", "e")}");
            });
        }
    }
}

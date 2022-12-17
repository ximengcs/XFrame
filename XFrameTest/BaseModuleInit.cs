using XFrame.Core;
using XFrame.Modules.Archives;
using XFrame.Modules.XType;

namespace XFrameTest
{
    [TestClass]
    public class BaseModuleInit
    {
        [TestMethod]
        public void Test1()
        {
            XCore core = XCore.Create(typeof(TypeModule));
            TypeModule.System system = TypeModule.Inst.GetOrNewWithAttr<ArchiveAttribute>();
            foreach(Type type in system)
                Console.WriteLine(type.Name);
        }
    }
}

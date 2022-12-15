using XFrame.Modules.XType;
using XFrame.Modules.Entities;

namespace XFrameTest
{
    [TestClass]
    public class TypeTest
    {
        [TestMethod]
        public void Test1()
        {
            new TypeModule().OnInit(null);
            TypeModule.System system = TypeModule.Inst.GetOrNew<Entity>();
            Console.WriteLine(system.Main.Name);
            foreach (Type type in system)
                Console.WriteLine(type.Name);

            Console.WriteLine("--------");
            TypeModule.System system2 = TypeModule.Inst.GetOrNewWithAttr<EntityPropAttribute>();
            Console.WriteLine(system2.Main.Name);
            foreach (Type type in system2)
                Console.WriteLine(type.Name);
        }
    }
}

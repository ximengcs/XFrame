using XFrame.Modules.XType;
using XFrame.Modules.Entities;

namespace XFrameTest
{
    public interface Type1
    {

    }

    public class Type2 : Type1
    {

    }

    [TestClass]
    public class TypeTest
    {
        [TestMethod]
        public void Test1()
        {
            new TypeModule().OnInit(null);
            TypeModule.System system = TypeModule.Inst.GetOrNew<Type1>();
            Console.WriteLine(system.Main.Name);
            Console.WriteLine(typeof(Type1).IsAssignableFrom(typeof(Type1)));
            Console.WriteLine(typeof(Type1).IsAssignableFrom(typeof(Type2)));
            Console.WriteLine(typeof(Entity).IsAssignableFrom(typeof(Scene)));
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

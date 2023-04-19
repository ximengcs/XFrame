using XFrame.Modules.Diagnotics;
using XFrame.Modules.Entities;

namespace XFrameTest
{
    [TestClass]
    public class EntityTest
    {
        [TestMethod]
        public void Test1()
        {
            EntryTest.Exec(100, () =>
            {
                E1 e1 = EntityModule.Inst.Create<E1>();
                e1.AddCom<C1>();
            });
        }
    }
}

using XFrame.Core;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Entities;
using XFrame.Modules.Tasks;

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
                E1 e1 = EntityModule.Inst.Create<E1>((entity) =>
                {
                    Log.Debug("OnEntityReady");
                    entity.SetData(Pair.Create("TestKey", "TestValue"));
                });
                e1.AddCom<C1>((com) =>
                {
                    e1.AddCom<C2>((com2) =>
                    {
                        com2.SetData("2");
                    });
                    com.SetData(1);
                });

                TaskModule.Inst.GetOrNew<DelayTask>()
                    .Add(1f, () =>
                    {
                        EntityModule.Inst.Destroy(e1);
                    }).Start();
            });
        }

        [TestMethod]
        public void Test2()
        {
            EntryTest.Exec(100, () =>
            {
                E1 e1 = EntityModule.Inst.Create<E1>((entity) =>
                {
                    Log.Debug("OnEntityReady");
                    entity.SetData(Pair.Create("TestKey", "TestValue"));
                });
                e1.AddCom<C1>((com) =>
                {
                    e1.AddCom<C2>(2, (com2) =>
                    {
                        com2.SetData("2");
                    });
                    com.SetData(1);
                });

                e1.RemoveCom<C2>(2);
                EntityModule.Inst.Destroy(e1);
            });
        }

        [TestMethod]
        public void Test3()
        {
            EntryTest.Exec(50, () =>
            {
                EntityModule.Inst.RegisterEntity<EBase>();
                EBase c3 = EntityModule.Inst.Create<EBase>(3);
            });
        }

        [TestMethod]
        public void Test4()
        {
            EntryTest.Exec(50, () =>
            {
                EntityModule.Inst.Create<E3>();
            });
        }
    }
}

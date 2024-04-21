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
                Log.Debug("-------------Create E1 Start-------------");
                E1 e1 = Entry.GetModule<IEntityModule>().Create<E1>((db) =>
                {
                    Log.Debug("DB OnReady");
                    db.SetData(Pair.Create("TestKey", "TestValue"));
                    db.SetData(1);
                    db.SetData("2");
                });
                Log.Debug("-------------Create E1 End-------------");

                /*
                XModule.Task.GetOrNew<ActionTask>()
                    .Add(1f, () =>
                    {
                        Log.Debug("-------------Destroy E1 Start-------------");
                        XModule.Entity.Destroy(e1);
                        Log.Debug("-------------Destroy E1 End-------------");

                        Log.Debug("-------------Create E1 Start Again-------------");
                        e1 = XModule.Entity.Create<E1>((db) =>
                        {
                            Log.Debug("DB OnReady");
                            db.SetData(Pair.Create("TestKey 2", "TestValue 2"));
                            db.SetData(3);
                            db.SetData("4");
                        });
                        Log.Debug("-------------Create E1 End Again-------------");
                    }).Start();*/
            });
        }

        [TestMethod]
        public void Test2()
        {
            EntryTest.Exec(100, () =>
            {
                E1 e1 = Entry.GetModule<IEntityModule>().Create<E1>((entity) =>
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
                Entry.GetModule<IEntityModule>().Destroy(e1);
            });
        }

        [TestMethod]
        public void Test3()
        {
            EntryTest.Exec(50, () =>
            {
                Entry.GetModule<IEntityModule>().RegisterEntity<EBase>();
                EBase c3 = Entry.GetModule<IEntityModule>().Create<EBase>(3);
            });
        }

        [TestMethod]
        public void Test4()
        {
            EntryTest.Exec(50, () =>
            {
                Entry.GetModule<IEntityModule>().Create<E3>();
            });
        }
    }
}
